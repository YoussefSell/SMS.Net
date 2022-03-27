module.exports = function (grunt) {
  // Project configuration.
  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),

    // clean the outputs folders
    clean: ['Assets/build/*', 'Assets/temp/'],

    // load images and fonts
    copy: {
      fonts: {
        expand: true,
        flatten: true,
        src: 'node_modules/@fortawesome/fontawesome-free/webfonts/*',
        dest: 'Assets/build/webfonts/',
      },
      images: {
        expand: true,
        flatten: true,
        src: 'Assets/src/images/**',
        dest: 'Assets/build/media/images/',
      },
    },

    // process css
    sass: {
      'app-styles': {
        files: {
          'Assets/temp/css/app.css': 'Assets/src/scss/styles.scss',
        },
      },
      'app-pages-messages-preview': {
        files: {
          'Assets/temp/css/messages.preview.page.min.css': 'Assets/src/scss/pages/messages/preview.page.scss',
        },
      },
    },

    // minify css files
    cssmin: {
      css: {
        files: {
          'Assets/build/css/app.min.css': 'Assets/temp/css/app.css',
          'Assets/build/css/messages.preview.page.min.css': 'Assets/temp/css/messages.preview.page.min.css',
          'Assets/build/css/vendor.min.css': ['node_modules/jquery-toast-plugin/dist/jquery.toast.min.css', 'node_modules/@fortawesome/fontawesome-free/css/all.min.css'],
        },
      },
    },

    // process js
    concat: {
      'vendor-js': {
        src: [
          'node_modules/@fortawesome/fontawesome-free/js/fontawesome.min.js',
          'node_modules/@popperjs/core/dist/umd/popper.min.js',
          'node_modules/bootstrap/dist/js/bootstrap.min.js',
          'node_modules/jquery/dist/jquery.min.js',
          'node_modules/moment/min/moment.min.js',
        ],
        dest: 'Assets/temp/js/vendor.js',
      },
      'jquery-validation': {
        src: [
          'node_modules/jquery-validation/dist/jquery.validate.min.js', //
          'node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js',
          'node_modules/jquery-ajax-unobtrusive/dist/jquery.unobtrusive-ajax.min.js',
        ],
        dest: 'Assets/temp/js/jquery.validation.js',
      },
      'app-js': {
        src: ['Assets/src/js/app.js'],
        dest: 'Assets/temp/js/app.js',
      },
      // the dashboard
      'app-js-pages-dashboard': {
        src: ['node_modules/chart.js/dist/chart.min.js', 'Assets/src/js/pages/dashboard.js'],
        dest: 'Assets/temp/js/dashboard.page.js',
      },
      // the messages management pages
      'app-js-pages-messages-index': {
        src: ['Assets/src/js/pages/messages/index.js'],
        dest: 'Assets/temp/js/messages.index.page.js',
      },
      'app-js-pages-messages-preview': {
        src: ['node_modules/jquery-toast-plugin/dist/jquery.toast.min.js', 'Assets/src/js/pages/messages/preview.js'],
        dest: 'Assets/temp/js/messages.preview.page.js',
      },
      'app-js-pages-messages-add': {
        src: ['Assets/src/js/pages/messages/add.js'],
        dest: 'Assets/temp/js/messages.add.page.js',
      },
      // the clients management pages
      'app-js-pages-clients-index': {
        src: ['node_modules/jquery-toast-plugin/dist/jquery.toast.min.js', 'Assets/src/js/pages/clients/index.js'],
        dest: 'Assets/temp/js/clients.index.page.js',
      },
      'app-js-pages-clients-preview': {
        src: ['Assets/src/js/libs/qrcode.min.js', 'Assets/src/js/pages/clients/preview.js'],
        dest: 'Assets/temp/js/clients.preview.page.js',
      },
      'app-js-pages-clients-add': {
        src: ['Assets/src/js/pages/clients/add.js'],
        dest: 'Assets/temp/js/clients.add.page.js',
      },
      'app-js-pages-clients-setup': {
        src: ['Assets/src/js/libs/qrcode.min.js', 'Assets/src/js/pages/clients/setup.js'],
        dest: 'Assets/temp/js/clients.setup.page.js',
      },
    },

    // minify js files
    uglify: {
      // external libs
      'vendor-js': {
        src: 'Assets/temp/js/vendor.js',
        dest: 'Assets/build/js/vendor.min.js',
      },
      'jquery-validation-js': {
        src: 'Assets/temp/js/jquery.validation.js',
        dest: 'Assets/build/js/jquery.validation.min.js',
      },
      // the dashboard
      'app-js': {
        src: 'Assets/temp/js/app.js',
        dest: 'Assets/build/js/app.min.js',
      },
      'app-js-pages-dashboard': {
        src: 'Assets/temp/js/dashboard.page.js',
        dest: 'Assets/build/js/dashboard.page.min.js',
      },
      // the messages management pages
      'app-js-pages-messages-add': {
        src: 'Assets/temp/js/messages.add.page.js',
        dest: 'Assets/build/js/messages.add.page.min.js',
      },
      'app-js-pages-messages-index': {
        src: 'Assets/temp/js/messages.index.page.js',
        dest: 'Assets/build/js/messages.index.page.min.js',
      },
      'app-js-pages-messages-preview': {
        src: 'Assets/temp/js/messages.preview.page.js',
        dest: 'Assets/build/js/messages.preview.page.min.js',
      },
      // the clients management pages
      'app-js-pages-clients-add': {
        src: 'Assets/temp/js/clients.add.page.js',
        dest: 'Assets/build/js/clients.add.page.min.js',
      },
      'app-js-pages-clients-index': {
        src: 'Assets/temp/js/clients.index.page.js',
        dest: 'Assets/build/js/clients.index.page.min.js',
      },
      'app-js-pages-clients-preview': {
        src: 'Assets/temp/js/clients.preview.page.js',
        dest: 'Assets/build/js/clients.preview.page.min.js',
      },
      'app-js-pages-clients-setup': {
        src: 'Assets/temp/js/clients.setup.page.js',
        dest: 'Assets/build/js/clients.setup.page.min.js',
      },
    },
  });

  // load plugins
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-sass');
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-uglify');
  grunt.loadNpmTasks('grunt-contrib-cssmin');

  // tasks aliases
  grunt.registerTask('clean_temp', 'clean:1');

  // Default task(s).
  grunt.registerTask('default', ['clean', 'copy', 'sass', 'cssmin', 'concat', 'uglify', 'clean_temp']);
};
