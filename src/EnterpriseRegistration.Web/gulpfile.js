/// <binding Clean='clean' />

var gulp = require("gulp"),
  rimraf = require("rimraf"),
  fs = require("fs"),
  stylus = require('gulp-stylus');

eval("var project = " + fs.readFileSync("./project.json"));

var paths = {
  bower: "./bower_components/",
  lib: "./" + project.webroot + "/lib/",
  stylus: "./Content/css/",
  //js: "./Content/js/"
};

gulp.task("clean", function (cb) {
  rimraf(paths.lib, cb);
});

gulp.task("copy", ["clean"], function () {
  var bower = {
    "bootstrap": "bootstrap/dist/**/*.{js,map,css,ttf,svg,woff,woff2,eot}",
    "jquery": "jquery/dist/jquery.min.{js,map}",
    "angular": ["angular/angular.min.js", "angular/angular.min.js.map"],
    "angular-ui-bootstrap-bower": "angular-ui-bootstrap-bower/ui-bootstrap-tpls.min.js",
    "angular-ui-grid":"angular-ui-grid/ui-grid.*",
    "angular-resource":["angular-resource/angular-resource.min.js","angular-resource/angular-resource.min.js.map"]
  }

  for (var destinationDir in bower) {
      if (typeof bower[destinationDir] === 'string') {
          gulp.src(paths.bower + bower[destinationDir])
              .pipe(gulp.dest(paths.lib + destinationDir));
      }
      else if (typeof bower[destinationDir] === 'object' && bower[destinationDir] instanceof Array) {
          gulp.src(bower[destinationDir].map(function (x) { return paths.bower + x; }))
             .pipe(gulp.dest(paths.lib + destinationDir));
      }
  }

  gulp.src(paths.js + '/*').pipe(gulp.dest('./' + project.webroot + "/js/"));
});

gulp.task("stylus", function () {
    gulp.src(paths.stylus +'*.styl')
    .pipe(stylus({
        'include css': true
    }))
    .pipe(gulp.dest('./' + project.webroot + '/css/'));
});
