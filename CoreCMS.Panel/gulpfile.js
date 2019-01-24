const 
    gulp = require('gulp'),
    sourcemaps = require('gulp-sourcemaps'),
    sass = require('gulp-sass'),
    uglify = require('gulp-uglify'),
    concat = require('gulp-concat'),
    path = require('path');


function getDestFolderFor(resource){
    const destFolder = './Assets/cms';
    return path.join(destFolder, resource);
}

//images
gulp.task('images', () => {
    return gulp.src('./Images/**/*')
    .pipe(gulp.dest(getDestFolderFor('images')));
});

//libs css
gulp.task('libs:css', () => {
    return gulp.src([
        './node_modules/bootstrap/dist/css/bootstrap.min.css'
    ])
    .pipe(gulp.dest(getDestFolderFor('libs/css')));
});

//libs js
gulp.task('libs:js', () => {
    return gulp.src([
        './node_modules/bootstrap/dist/js/bootstrap.min.js',
        './node_modules/jquery/dist/jquery.min.js'
    ])
    .pipe(gulp.dest(getDestFolderFor('libs/js')));
});

//libs
gulp.task('libs', gulp.parallel('libs:js', 'libs:css'));

//styles
const stylesBlob = './Styles/**/*.scss';
gulp.task('watch:css', () => {
    return gulp.watch(stylesBlob, gulp.parallel('css'));
});
gulp.task('css', () => {
    return gulp.src(stylesBlob)
    .pipe(sourcemaps.init())
    .pipe(sass({outputStyle: 'compressed'}).on('error', sass.logError))
    .pipe(concat('index.css'))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(getDestFolderFor('css')));
});

//scripts
const scriptsBlob = './Scripts/**/*.js';
gulp.task('watch:js', () => {
    return gulp.watch(scriptsBlob, gulp.parallel('js'));
});
gulp.task('js', () => {
    return gulp.src(scriptsBlob)
    .pipe(sourcemaps.init())
    .pipe(concat('index.js'))
    .pipe(uglify())
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(getDestFolderFor('js')));
});

//build
gulp.task('build', gulp.parallel('libs', 'images', 'css', 'js'));

//watch
gulp.task('watch', gulp.parallel('build', 'watch:js', 'watch:css'));