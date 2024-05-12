var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').dataTable({
        "ajax": { url: "/order/getall" },
        "columns": [
            { data: 'orderheaderid', "width": "5%"},
            { data: 'email', "width": "25%"}
        ]
    })
}