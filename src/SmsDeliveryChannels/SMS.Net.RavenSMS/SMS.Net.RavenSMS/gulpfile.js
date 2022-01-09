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

// js
gulp.task('js-vendor', function(){
	return gulp.src([
		'node_modules/@popperjs/core/dist/umd/popper.min.js',
		'node_modules/bootstrap/dist/js/bootstrap.bundle.min.js',
		'node_modules/@fortawesome/fontawesome-free/js/all.min.js',
	])
	.pipe(sourcemaps.init())
	.pipe(concat('vendor.min.js'))
	.pipe(sourcemaps.write())
	.pipe(gulp.dest('Assets/js'));
});

gulp.task('js-app', function(){
	return gulp.src(['Assets/src/js/app.js'])
		.pipe(sourcemaps.init())
		.pipe(concat('app.min.js'))
		.pipe(sourcemaps.write())
		.pipe(gulp.dest('Assets/js'));
});

gulp.task('js-app-pages-dashboard', function () {
	return gulp.src([
		'node_modules/chartjs/chart.js',
		'Assets/src/js/dashboard.page.js',
	])
	.pipe(sourcemaps.init())
	.pipe(concat('dashboard.page.min.js'))
	.pipe(sourcemaps.write())
	.pipe(gulp.dest('Assets/js'));
});

// runner
gulp.task('default', gulp.series(gulp.parallel([
	// default
	'images',

	// css tasks
	'css-vendor',
	'css-app',

	// js tasks
	'js-vendor',
	'js-app',

	// pages scripts
	'js-app-pages-dashboard'
])));