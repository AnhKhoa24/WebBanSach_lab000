document.addEventListener("DOMContentLoaded", function () {

    //var idchuden = $("#chuDe .table-active").data('idchude');
    getdata();
    $('#chuDe').on('click', 'tr', function () {
        if ($(this).hasClass('table-active')) {
            $(this).removeClass('table-active');
            getdata();
        } else {
            $('#chuDe tr').removeClass('table-active');
            $(this).addClass('table-active');
            getdata();
        }
    });
    $('#nhaXuatBan').on('click', 'tr', function () {
        if ($(this).hasClass('table-active')) {
            $(this).removeClass('table-active');
            getdata();
        } else {
            $('#nhaXuatBan tr').removeClass('table-active');
            $(this).addClass('table-active');
            getdata();
        }
    });

});

function getdata() {
    $.ajax({
        url: '/Home/GetDSsach',     
        type: 'POST',        
        dataType: 'json',   
        data: {         
            idcd: $("#chuDe .table-active")?.data('idchude')??0,
            idnxb: $("#nhaXuatBan .table-active")?.data('idchude')??0,
        },
        success: function (response) {
            $("#DanhSach").empty();
            response.forEach(function (element) {

                var item = `<div class="col-3">
                    <div class="card border-0 shadow-sm">
                        <img src="/img/anhbia1.jpg" class="card-img-top" style="height: 200px; object-fit: cover;" alt="Sách">
                        <div class="card-body text-center">
                            <h5 class="card-title">${element.tensach}</h5>
                            <p class="card-text text-muted">Giá: ${element.giaban} VND</p>
                            <a href="#" class="btn btn-primary">Mua ngay</a>
                        </div>
                    </div>
                </div>`;
                $("#DanhSach").append(item);
            });
        },
        error: function (xhr, status, error) {
            console.error('Lỗi:', status, error);
        }
    });

}
