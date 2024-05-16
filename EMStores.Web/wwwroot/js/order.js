$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    let dataTable = new DataTable('#tblData');
    dataTable.dataTable({
        "ajax": { url: "/order/getall" , type: "GET" },
        "columns": [
            { data: 'orderheaderid', "width": "5%"},
            { data: 'Email', "width": "25%"}
        ]
    })
}