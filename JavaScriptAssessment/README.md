# JavaScript Assessment
> This is the NHLPA software developer test. Any information contained within is completely fictitious.

## The Task

Using the provided `index.html` file, within the script tag immediately preceding the `</body>` tag. Complete the following steps without the aid of jQuery or any other external libraries. Modifying the markup directly is not permitted. Targetting only greenfield browser's is acceptable.

1. Set the global global font family to: `font-family: sans-serif`
2. Hide (i.e. `display: none`) any element(s) with the class: `.instructions`</li>
3. Execute AJAX request `GET https://jsonplaceholder.typicode.com/photos/1`. On success, append `<img src="{thumbnailUrl}" />` to the body.</li>
4. Execute AJAX request `GET https://jsonplaceholder.typicode.com/posts?userId=1`. On success, for each item returned, append `<a href="https://jsonplaceholder.typicode.com/posts/{id}">{title}<a/>` to the body.</li>

