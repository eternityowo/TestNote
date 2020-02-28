// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(setTimezoneCookie());
function setTimezoneCookie() {

    var timezone_cookie = "timezoneoffset";


    console.log(timezone_cookie);
    // if the timezone cookie not exists create one.
    if (!Cookies.get(timezone_cookie)) {

        // check if the browser supports cookie
        var test_cookie = 'test cookie';
        Cookies.set(test_cookie, true);

        // browser supports cookie
        if (Cookies.get(test_cookie)) {

            // delete the test cookie
            //$.cookie(test_cookie, null);

            // create a new cookie 
            Cookies.set(timezone_cookie, new Date().getTimezoneOffset());

            // re-load the page
            location.reload();
        }
    }
    // if the current timezone and the one stored in cookie are different
    // then store the new timezone in the cookie and refresh the page.
    else {

        var storedOffset = parseInt(Cookies.get(timezone_cookie));
        var currentOffset = new Date().getTimezoneOffset();

        // user may have changed the timezone
        if (storedOffset !== currentOffset) {
            Cookies.set(timezone_cookie, new Date().getTimezoneOffset());
            location.reload();
        }
    }
}