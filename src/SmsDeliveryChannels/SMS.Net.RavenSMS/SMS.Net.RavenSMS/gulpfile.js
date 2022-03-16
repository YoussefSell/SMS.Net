var gulp        = require('gulp');
var sass		= require('gulp-sass')(require('sass'));
var minifyCSS   = require('gulp-clean-css');
var concat      = require('gulp-concat');
var sourcemaps  = require('gulp-sourcemaps');
var download    = require('gulp-download2');
var uglify      = require('gulp-uglify');

// images and fonts
gulp.task('fonts', function () {
	return gulp.src(['node_modules/@fortawesome/fontawesome-free/webfonts/*'])
		.pipe(gulp.dest('Assets/webfonts/'));
});

gulp.task('images', function () {
	return gulp.src(['Assets/src/images/**'])
		.pipe(gulp.dest('Assets/media/images/'));
});

// css
gulp.task('css-vendor', function () {
	return gulp.src([
		'node_modules/jquery-toast-plugin/dist/jquery.toast.min.css',
		'node_modules/@fortawesome/fontawesome-free/css/all.min.css',
	])
	.pipe(sass())
	.pipe(concat('vendor.min.css'))
	.pipe(minifyCSS())
	.pipe(gulp.dest('Assets/css/'));
});

gulp.task('css-app', function () {
	return gulp.src([
		'Assets/src/scss/styles.scss'
	])
	.pipe(sass())
	.pipe(concat('app.min.css'))
	.pipe(minifyCSS())
	.pipe(gulp.dest('Assets/css/'));
});

// message pages css
gulp.task('css-app-pages-messages-preview', function () {
	return gulp.src([
		'Assets/src/scss/pages/messages/preview.page.scss',
	])
	.pipe(sass())
	.pipe(concat('messages.preview.page.min.css'))
	.pipe(minifyCSS())
	.pipe(gulp.dest('Assets/css/'));
});

// js
gulp.task('js-vendor', function(){
	return gulp.src([
		'node_modules/jquery/dist/jquery.min.js',
		'node_modules/@popperjs/core/dist/umd/popper.min.js',
		'node_modules/bootstrap/dist/js/bootstrap.bundle.min.js',
		'node_modules/@fortawesome/fontawesome-free/js/all.min.js',
	])
	.pipe(sourcemaps.init())
	.pipe(uglify())
	.pipe(concat('vendor.min.js'))
	.pipe(sourcemaps.write())
	.pipe(gulp.dest('Assets/js'));
});

gulp.task('js-vendor-jquery-validation', function () {
	return gulp.src([
		'node_modules/jquery-validation/dist/jquery.validate.min.js',
		'node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js',
		'node_modules/jquery-ajax-unobtrusive/dist/jquery.unobtrusive-ajax.min.js',
	])
	.pipe(sourcemaps.init())
	.pipe(uglify())
	.pipe(concat('vendor.jquery.validation.min.js'))
	.pipe(sourcemaps.write())
	.pipe(gulp.dest('Assets/js'));
});

gulp.task('js-app', function(){
	return gulp.src(['Assets/src/js/app.js'])
		.pipe(sourcemaps.init())
		.pipe(uglify())
		.pipe(concat('app.min.js'))
		.pipe(sourcemaps.write())
		.pipe(gulp.dest('Assets/js'));
});

gulp.task('js-app-pages-dashboard', function () {
	return gulp.src([
		'node_modules/chart.js/dist/chart.min.js',
		'Assets/src/js/pages/dashboard.js',
	])
	.pipe(sourcemaps.init())
	.pipe(uglify())
	.pipe(concat('dashboard.page.min.js'))
	.pipe(sourcemaps.write())
	.pipe(gulp.dest('Assets/js'));
});

// the message management pages
gulp.task('js-app-pages-messages-add', function () {
	return gulp.src(['Assets/src/js/pages/messages/add.js'])
	.pipe(sourcemaps.init())
	.pipe(uglify())
	.pipe(concat('messages.add.page.min.js'))
	.pipe(sourcemaps.write())
	.pipe(gulp.dest('Assets/js'));
});

gulp.task('js-app-pages-messages-index', function () {
	return gulp.src([
		'node_modules/moment/min/moment.min.js',
		'Assets/src/js/pages/messages/index.js'
	])
	.pipe(sourcemaps.init())
	.pipe(uglify())
	.pipe(concat('messages.index.page.min.js'))
	.pipe(sourcemaps.write())
	.pipe(gulp.dest('Assets/js'));
});

gulp.task('js-app-pages-messages-preview', function () {
	return gulp.src([
		'node_modules/jquery-toast-plugin/dist/jquery.toast.min.js',
		'Assets/src/js/pages/messages/preview.js',
	])
		.pipe(sourcemaps.init())
		.pipe(uglify())
		.pipe(concat('messages.preview.page.min.js'))
		.pipe(sourcemaps.write())
		.pipe(gulp.dest('Assets/js'));
});

// the clients management pages
gulp.task('js-app-pages-clients-index', function () {
	return gulp.src([
		'node_modules/jquery-toast-plugin/dist/jquery.toast.min.js',
		'Assets/src/js/pages/clients/index.js',
	])
	.pipe(sourcemaps.init())
	.pipe(uglify())
	.pipe(concat('clients.index.page.min.js'))
	.pipe(sourcemaps.write())
	.pipe(gulp.dest('Assets/js'));
});

gulp.task('js-app-pages-clients-add', function () {
	return gulp.src(['Assets/src/js/pages/clients/add.js'])
		.pipe(sourcemaps.init())
		.pipe(uglify())
		.pipe(concat('clients.add.page.min.js'))
		.pipe(sourcemaps.write())
		.pipe(gulp.dest('Assets/js'));
});

gulp.task('js-app-pages-clients-setup', function () {
	return gulp.src([
			'Assets/src/js/libs/qrcode.min.js',
			'Assets/src/js/pages/clients/setup.js',
		])
		.pipe(sourcemaps.init())
		.pipe(uglify())
		.pipe(concat('clients.setup.page.min.js'))
		.pipe(sourcemaps.write())
		.pipe(gulp.dest('Assets/js'));
});

gulp.task('js-app-pages-clients-preview', function () {
	return gulp.src([
		'Assets/src/js/libs/qrcode.min.js',
		'Assets/src/js/pages/clients/preview.js',
	])
		.pipe(sourcemaps.init())
		.pipe(uglify())
		.pipe(concat('clients.preview.page.min.js'))
		.pipe(sourcemaps.write())
		.pipe(gulp.dest('Assets/js'));
});

// runner
gulp.task('default', gulp.series(gulp.parallel([
	// default
	'images', 'fonts',

	// css tasks
	'css-vendor',
	'css-app',

	// pages styles
	// messages
	'css-app-pages-messages-preview',

	// js tasks
	'js-app',
	'js-vendor',
	'js-vendor-jquery-validation',

	// pages scripts
	// dashboard
	'js-app-pages-dashboard',

	// messages
	'js-app-pages-messages-add',
	'js-app-pages-messages-index',
	'js-app-pages-messages-preview',

	// clients
	'js-app-pages-clients-add',
	'js-app-pages-clients-index',
	'js-app-pages-clients-setup',
	'js-app-pages-clients-preview',
])));