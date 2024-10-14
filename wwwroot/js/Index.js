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
function addtoCart(masach) {
    var token = getCookie('access_token');
    if (token) {
        $.ajax({
            url: '/MuaHang/addToCart',
            type: 'POST',
            headers: {
                'Authorization': 'Bearer ' + token
            },
            data:{
                ma_Sach: masach,
                sl: 1
            },
            success: function (response) {
                swal("Thành công!", "Bạn đã thêm vào giỏ hàng!", "success");
            },
            error: function (error) {
                console.error(error);
            }
        });
    } else {
        var modal = new bootstrap.Modal($('#loginModal')[0]);
        modal.show();
    }
}

function getCart(show) {
    var token = getCookie('access_token');
    if (token) {
        $.ajax({
            url: '/MuaHang/getCart',
            type: 'POST',
            headers: {
                'Authorization': 'Bearer ' + token
            },
            success: function (response) {

                $("#cartInCanvas").empty();
                var tongtienf = 0;
                response.forEach(function (element) {
                    var formattedPrice = new Intl.NumberFormat('en-US').format(element.gia);
                    var formattedTotal = new Intl.NumberFormat('en-US').format(element.tongtien);
                    var item = `<tr>
                        <td>
                            <img src="/img/anhbia1.jpg" class="img-fluid" style="height:150px;max-width: 100px;">
                        </td>
                      <td style="width: 230px; height: 150px;">
                        <div>
                            <strong>${element.tensach}</strong> 
                            </div>
                             <div class="mt-2">
                            <strong>SL</strong>
                            <input type="number" class="form-control d-inline-block" value="${element.sl}" min="1" style="width: 70px;">
                        </div>
                        <div class="mt-2">
                            <strong>Giá:</strong> <span>${formattedPrice} VNĐ</span>
                        </div>
                        <div class="mt-2">
                            <strong>Tổng:</strong> <span>${formattedTotal} VNĐ</span>
                        </div>
                        </td>
                        <td>
                            <button class="btn btn-danger btn-sm" onclick="removeItem(${element.id})">Xóa</button>
                        </td>
                    </tr>`;
                    $("#cartInCanvas").append(item);
                    tongtienf += element.tongtien;
                });
                var formattedTotalf = new Intl.NumberFormat('en-US').format(tongtienf);
                $("#cartInCanvas").append(`<tr style="width: 230px;">
                        <td  class="text-end"><strong>Tổng tiền:</strong></td>
                        <td colspan="2"><strong>${formattedTotalf} VNĐ</strong></td>
                    </tr>
                    `);

                if (response.length > 0) {
                    $("#btnThanhToan").prop("disabled", false);
                } else {
                    $("#btnThanhToan").prop("disabled", true);
                }


                if (show) {
                    var modal = new bootstrap.Offcanvas($('#giohangCanvas')[0]);
                    modal.show();
                }
            },
            error: function (error) {
                console.error(error);
            }
        });
    } else {
        var modal = new bootstrap.Modal($('#loginModal')[0]);
        modal.show();
    }
}
function removeItem(id)
{
    $.ajax({
        url: '/MuaHang/removeCartItem',
        type: 'POST',
        headers: {
            'Authorization': 'Bearer ' + getCookie('access_token')
        },
        data: {
            idCart:id
        },
        success: function (response) {
            getCart()
        },
        error: function (error) {
            console.error(error);
        }
    });
}