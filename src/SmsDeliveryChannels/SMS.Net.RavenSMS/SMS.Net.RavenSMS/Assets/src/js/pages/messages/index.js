$('a[data-bs-toggle="tab"]').on('click', function (e) {
  loadData();
});

// init the date range picker
$('#search-period').daterangepicker(
  {
    ranges: {
      Today: [moment(), moment()],
      'Last 7 Days': [moment().subtract(6, 'days'), moment()],
      'Last 30 Days': [moment().subtract(29, 'days'), moment()],
      'This Month': [moment().startOf('month'), moment().endOf('month')],
      'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
    },
  },
  setDateRange
);

// init the required variables
let messages = [];
let startDate = null;
let endDate = null;

// set the date range initial value
setDateRange(moment().subtract(29, 'days'), moment());

// load the initial data
loadData();

function loadData() {
  $.ajax({
    url: '/ravenSMS/messages/index/?handler=Messages',
    data: {
      pageIndex: 1,
      pageSize: 5,
      searchQuery: $('#search-query').val(),
      endDate: endDate.format('yyyy-MM-DDTHH:mm:ssZ'),
      startDate: startDate.format('yyyy-MM-DDTHH:mm:ssZ'),
      Status: $('nav#messages-status a.active').attr('value'),
      to: [],
      from: [],
      clients: [],
    },
    success: function (result) {
      // save the result
      messages = result.data;

      // select table element
      const $table = $('#messages_table');

      // empty the table
      $('#messages_table > tbody').empty();

      // add the data to the table
      $.each(messages, function () {
        $table.append(buildTableRow(this));
      });

      // set the pagination details
      SetPagination(result.pagination.rowsCount, result.pagination.pageIndex, result.pagination.pageSize);
    },
  });
}

function buildTableRow(message) {
  const messageDate = moment(message.date);

  return `<tr>
            <td style="max-width:150px" class="cell">${message.id}</td>
            <td class="cell"><span class="truncate">${message.client.name}</span></td>
            <td class="cell">${message.to}</td>
            <td class="cell"><span>${messageDate.format('DD/MMM/YYYY')}</span><span class="note">${messageDate.format('hh:mm A')}</span></td>
            <td class="cell">${GetStatusSpan(message.status)}</td>
            <td class="cell">
                <a class="btn-sm app-btn-secondary" href="Messages/Preview/${message.id}">View</a>
            </td>
        </tr>`;
}

function SetPagination(rowsCount, pageIndex, pageSize) {
  // select the paginator
  const $paginator = $('#paginator');

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
  });

  // append the Next button
  $paginator.append(`
            <li class="page-item">
                <a class="page-link" href="#">Next</a>
            </li>
        `);
}

function GetStatusSpan(status) {
  switch (status) {
    case 1:
      return `<span class="badge bg-warning">Queued</span>`;
    case 2:
      return `<span class="badge bg-danger">Failed</span>`;
    case 3:
      return `<span class="badge bg-success">Sent</span>`;
    default:
      return `<span class="badge bg-secondary">Created</span>`;
  }
}

function setDateRange(start, end) {
  // set the values of the start & end dates
  startDate = start;
  endDate = end;

  // set the date range text
  $('#search-period span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
}
