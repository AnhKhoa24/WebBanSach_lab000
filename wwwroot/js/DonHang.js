
$(document).ready(function () {
    getDonHang(1);
    $('#phantrang').on("click", ".page-item:not(.disabled)", function (event) {
        event.preventDefault();
        var pageNum;
        if ($(this).hasClass('previous')) {
            var currentPage = $('#phantrang .page-item.active').find('.page-link').text();
            pageNum = parseInt(currentPage) - 1;
        } else if ($(this).hasClass('next')) {
            var currentPage = $('#phantrang .page-item.active').find('.page-link').text();
            pageNum = parseInt(currentPage) + 1;
        } else {
            pageNum = $(this).find('.page-link').text();
        }
        if (!$(this).hasClass('active')) {
            //$('#phantrang .page-item').removeClass('active');
            //$(this).addClass('active');
            getDonHang(pageNum);
        }
    });
});

function getDonHang(page) {
    $.ajax({
        url: '/Admin/getDonHang',
        type: 'POST',
        dataType: 'json',
        data: {
            trang: page,
            pagesize: 10,
            search: ''
        },
        success: function (response) {
            $("#dataDonHang").empty();
            var i = 1;
            response.data.forEach(function (element) {
                var sps = '';
                var thanhtien = 0;
                element.sanpham.forEach(function (item) {
                    var giasach = new Intl.NumberFormat('en-US').format(item.dongia);
                    sps += `<li>${item.tensach} - ${item.soluong} cuốn - ${giasach} đ</li>`;
                    thanhtien += item.thanhTien;
                });
                var giattf = new Intl.NumberFormat('en-US').format(thanhtien);
                sps += `<li>Tổng tiền: ${giattf} đ</li>`;
                var item = `
                <tr data-toggle="collapse" data-target="#row-${i}" aria-expanded="false" aria-controls="row2">
                            <td>${i}</td>
                            <td>${element.maDonHang}</td>
                            <td>${element.ngaydat}</td>
                            <td>${element.ngaygiao}</td>
                            <td>${element.tinhtranggiaohang}</td>
                        </tr>
                        <tr class="collapse" id="row-${i}">
                            <td colspan="2">
                                <div class="p-3">
                                    <strong>Thông tin người nhận</strong>
                                    <br />
                                    <span>${element.hoTen}</span>
                                    <br />
                                    <span>SĐT: ${element.dienthoaiKh}</span>
                                    <br />
                                    <span>ĐC: ${element.diachiKh}</span>
                                </div>
                            </td>
                            <td colspan="3">
                                <div class="p-3">
                                    <strong>Chi tiết đơn hàng:</strong>
                                    <ul>
                                       ${sps}
                                    </ul>
                                </div>
                            </td>
                        </tr>
               `;
                $("#dataDonHang").append(item);
                i++;
            });
            GenPagnation(response.trang, response.tongtrang);
        },
        error: function (xhr, status, error) {
            alert("Sai mật khẩu");
        }
    });
}

function GenPagnation(trang, sotrang) {
    var htmltrang = "";
    if (trang == 1) {
        htmltrang += `<li class="page-item disabled previous">
                        <a class="page-link" href="#" tabindex="-1" aria-disabled="true">Previous</a>
                    </li>`;
    } else {
        htmltrang += `<li class="page-item previous">
                        <a class="page-link" href="#">Previous</a>
                    </li>`;
    }

    for (let i = 1; i <= sotrang; i++) {
        if (i == trang) {
            htmltrang += `<li class="page-item active"><a class="page-link" href="#">${i}</a></li>`;
        } else {
            htmltrang += `<li class="page-item"><a class="page-link" href="#">${i}</a></li>`;
        }
    }

    if (trang == sotrang) {
        htmltrang += `<li class="page-item disabled next">
                        <a class="page-link" href="#">Next</a>
                    </li>`;
    } else {
        htmltrang += `<li class="page-item next">
                        <a class="page-link" href="#">Next</a>
                    </li>`;
    }

    $("#phantrang").empty();
    $("#phantrang").append(htmltrang);
}
