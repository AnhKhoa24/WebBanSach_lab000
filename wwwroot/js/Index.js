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
                var formattedPrice = new Intl.NumberFormat('en-US').format(element.giaban);

                var item = `<div class="col-3">
                    <div class="card border-0 shadow-sm">
                    <img onclick="showdetail(${element.masach})" src="/img/anhbia1.jpg" class="card-img-top" style="height: 200px; object-fit: cover;" alt="Sách">
                        <div class="card-body text-center">
                            <h5 class="card-title">${element.tensach}</h5>
                             <p class="card-text text-muted">Giá: ${formattedPrice} VND</p>
                            <button type="button" onclick="addtoCart(${element.masach})" class="btn btn-primary">
                             <i class="fas fa-shopping-cart"></i> Thêm vào giỏ hàng
                            </button>
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
function showdetail(masach) 
{
    console.log(masach)
    $.ajax({
        url: '/Home/getDetailSach',
        type: 'POST',
        dataType: 'json',
        data: {
            masach: masach
        },
        success: function (response) {
            var formattedPrice = new Intl.NumberFormat('en-US').format(response.giaban);
            $('#productName').text(response.tensach);
            $('#productPrice').text(`Giá: ${formattedPrice} VND`);
            $('#productDescription').text(response.mota);
            $('#productQuantity').html(`Số lượng còn lại: <span class="fw-bold">${response.soluongton}</span>`);
            $('#nutthemmodal').html(` <button type="button" onclick="addtoCart(${response.masach})" class="btn btn-primary">
                             <i class="fas fa-shopping-cart"></i> Thêm vào giỏ hàng
                            </button>`);
            var modal = new bootstrap.Modal($('#productModal')[0]);
            modal.show();
        },
        error: function (xhr, status, error) {
            console.error('Lỗi:', status, error);
        }
    });
}
function addtoCart(masach)
{
    swal("Thành công!", "Bạn thêm vào giỏ hàng!", "success");
}