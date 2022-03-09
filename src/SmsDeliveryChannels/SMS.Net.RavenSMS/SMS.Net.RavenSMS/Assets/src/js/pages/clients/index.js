// init the status tabs click event
$('a[data-bs-toggle="tab"]').on('click', function (e) {
    loadData();
});

// init the required variables
let clients = [];

// load the initial data
loadData();

function loadData() {
    $.ajax({
        url: '/ravenSMS/clients/index/?handler=clients',
        data: {
            pageIndex: 1,
            pageSize: 5,
            searchQuery: $("#search-query").val(),
            Status: $('nav#clients-status a.active').attr('value'),
            phoneNumbers: [],
        },
        success: function (result) {
            // save the result
            clients = result.data;

            // select table element
            const $table = $("#clients_table");

            // empty the table
            $("#clients_table > tbody").empty();

            // add the data to the table
            $.each(clients, function () {
                $table.append(buildTableRow(this));
            });

            // set the pagination details
            SetPagination(
                result.pagination.rowsCount,
                result.pagination.pageIndex,
                result.pagination.pageSize,
            );
        }
    });
}

function buildTableRow(client) {
    let markup = `<tr>
        <td style="max-width:150px" class="cell">${client.id}</td>
        <td class="cell"><span class="truncate">${client.name}</span></td>
        <td class="cell">${GetStatusSpan(client.status)}</td>
        <td class="cell fit">`;

    // add the preview page if the client has been configured
    if (client.status != 3) {
        markup += `<a class="btn-sm app-btn-secondary" href="Clients/Preview/${client.id}">View</a>`;
    }

    // add the setup page id the client is not configured
    if (client.status == 3) {
        markup += `<a class="btn-sm app-btn-secondary ms-2" href="Clients/Setup/${client.id}">Setup</a>`;
    }

    // add delete button
    markup += `<button class="btn-sm app-btn-secondary ms-2" onClick="deleteClient('${client.id}')">Delete</button></td></tr>`;

    return markup;
}

function SetPagination(rowsCount, pageIndex, pageSize) {
    // select the paginator
    const $paginator = $("#paginator");

    // empty the paginator
    $paginator.empty();

    // append the previous button
    $paginator.append(`
            <li class="page-item disabled">
                <a class="page-link" href="#" tabindex="-1" aria-disabled="true">Previous</a>
            </li>
        `);

    // get the pagination logic
    const pagination = paginate(rowsCount, pageIndex, pageSize);

    // append pages
    $.each(pagination.pages, function () {
        $paginator.append(`<li class="page-item ${this == pagination.currentPage ? 'active' : ''}"><a class="page-link" href="#">${this}</a></li>`);
    })

    // append the Next button
    $paginator.append(`
            <li class="page-item">
                <a class="page-link" href="#">Next</a>
            </li>
        `);
}

function GetStatusSpan(status) {
    switch (status) {
        case 0: return `<span class="badge bg-warning">UnActive</span>`
        case 1: return `<span class="badge bg-success">Connected</span>`
        case 2: return `<span class="badge bg-danger">Disconnected</span>`
        default: return `<span class="badge bg-danger">Require setup</span>`
    }
}

function deleteClient(clientId) {
    $.get('/ravenSMS/clients/index/?handler=RemoveClient&clientId=' + clientId)
        .done(function (result) {
            if (result.isSuccess) {
                $.toast({
                    heading: 'Success',
                    text: 'client has been deleted successfully.',
                    showHideTransition: 'slide',
                    position: 'top-right',
                    icon: 'success'
                });

                loadData();
                return;
            }

            $.toast({
                heading: 'Failed to resend',
                text: '<p>failed to delete the client, error message:<br/>' + result.error + '</p>',
                showHideTransition: 'fade',
                position: 'top-right',
                icon: 'error'
            });
        });
}