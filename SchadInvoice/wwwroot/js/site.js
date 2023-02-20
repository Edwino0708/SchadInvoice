// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function hide_table_elements(table_id, visible) {
    // Shows only the first `visible` table elements
    table_elements = document.getElementById(table_id).children[1].children

    for (const element of table_elements) {
        if (visible == 0) {
            element.style.display = 'none'
        }
        else {
            element.style.display = 'table-row'
            visible -= 1
        }
    }
}

// Use below solution for <td> without `value` attribute
// const getCellValue = (tr, idx) => tr.children[idx].innerText.replace('$', '') || tr.children[idx].textContent.replace('$', '');
const getCellValue = (tr, idx) => tr.children[idx].getAttribute('value')

const comparer = (idx, asc) => (a, b) =>
    ((v1, v2) => v1 !== '' && v2 !== '' && !isNaN(v1) && !isNaN(v2) ? v1 - v2 : v1.toString().localeCompare(v2))
        (getCellValue(asc ? a : b, idx), getCellValue(asc ? b : a, idx))

const reload_table = () => hide_table_elements('example', document.getElementById('form-select-coins').value)

window.addEventListener('load', function () {
    reload_table()

    // Show per page
    document.getElementById('form-select-coins').addEventListener('change', function () {
        counter = this.value
        hide_table_elements('example', this.value)
    });

    // Search in table
    document.getElementById('search-in-table').addEventListener('input', function () {
        rows = document.getElementById('example').children[1].querySelectorAll('tr:nth-child(n)')
        value = this.value.toLowerCase()

        str = JSON.stringify(row);
        console.log(str);

        if (value == '')
            return reload_table()

        for (const row of rows) {

            if (row.getAttribute('rowData').toLowerCase().includes(value)) {
                row.style.display = 'table-row'
                console.log("Yes:" + row);

            } else {
                console.log("No:" + row);
                row.style.display = 'none'
            }
        }
    });

    // Sort table
    document.querySelectorAll('th').forEach(th => th.addEventListener('click', (() => {
        const table = document.getElementById('example').children[1]
        str = JSON.stringify(table);
        console.log(str);

        Array.from(table.querySelectorAll('tr:nth-child(n)'))
            .sort(comparer(Array.from(th.parentNode.children).indexOf(th), this.asc = !this.asc))
            .forEach(tr => table.appendChild(tr));

        reload_table()
    })));
});
