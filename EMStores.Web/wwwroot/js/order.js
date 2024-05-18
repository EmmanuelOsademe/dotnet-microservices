let dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    /*let dataTable = new DataTable('#tblData');*/

    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: "/order/getall",
            type: "GET"
        },
        columns: [
            { data: "orderHeaderId", "width": "5%" },
            { data: "email", width: "10%"},
            { data: "name", "width": "25%" },
            { data: "phone", width: "15%" },
            { data: "status", width: "15%" },
            { data: "orderTotal", width: "15%" },
            { data: "_", width: "15%" },
        ]
    })
}