document.addEventListener("DOMContentLoaded", function () {


    $("#btnLogin").on("click", function () {
        login()
    });
    checkUserLoggedIn();

});

function login() {
    $.ajax({
        url: '/Auth/Login',
        type: 'POST',
        dataType: 'json',
        data: {
            name: $("#exampleInputEmail1").val(),
            password: $("#exampleInputPassword1").val()
        },
        success: function (response) {
            if (response.token) {
                setCookie('access_token', response.token, 1); 
            }
            checkUserLoggedIn();
            $('#loginModal').modal('hide');
            swal({
                title: "Thành công!",
                text: "Đã đăng nhập thành công",
                icon: "success",
                button: "Ok em!",
            });
        },
        error: function (xhr, status, error) {
            console.error('Lỗi:', status, error);
            $("#loginError").removeClass('d-none');
        }
    });
}

function setCookie(name, value, days) {
    let expires = "";
    if (days) {
        let date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}
function getCookie(name) {
    let cookieArr = document.cookie.split(";");

    for (let i = 0; i < cookieArr.length; i++) {
        let cookiePair = cookieArr[i].split("=");

        if (name == cookiePair[0].trim()) {
            return decodeURIComponent(cookiePair[1]);
        }
    }

    return null;
}
function checkUserLoggedIn() {
    var token = getCookie('access_token');

    if (token) {
        var userInfo = jwt_decode(token);

        $("#inforuser").removeClass('d-none');
        $("#logindothoi").addClass('d-none');
        //document.getElementById('user-info').innerText = `Xin chào, ${userInfo.username}`;
        console.log("Người dùng đã đăng nhập:", userInfo);

        $("#intro").text(`Chào ${userInfo.username}`)

    } else {
        console.log("Người dùng chưa đăng nhập.");
    }
}
function logout() {

    swal({
        title: "Đăng xuất?",
        text: "Bạn có chắc muốn đăng xuất không!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                document.cookie = "access_token=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC;";
                $("#inforuser").addClass('d-none');
                $("#logindothoi").removeClass('d-none');
                console.log("Đã đăng xuất, cookie token đã bị xóa.");
            } else {
                return
            }
        });
}

function thanhtoan() {

    $.ajax({
        url: '/MuaHang/getInfo',
        type: 'POST',
        headers: {
            'Authorization': 'Bearer ' + getCookie('access_token')
        },
        success: function (response) {
            $("#nameNN").val(response.hoTen)
            $("#emailNN").val(response.email)
            $("#sdtNN").val(response.dienthoaiKh)
            $("#diachiNN").text(response.diachiKh)
        },
        error: function (error) {
            console.error(error);
        }
    });

    var modal = new bootstrap.Modal($('#thongtindathang')[0]);
    modal.show();
}

function doithongtin() {
    swal({
        title: "Đang cập nhật thông tin...",
        text: "Vui lòng chờ...",
        icon: "info",
        buttons: false,
        closeOnClickOutside: false,
        closeOnEsc: false
    });

    $.ajax({
        url: '/MuaHang/updateInfo',
        type: 'POST',
        headers: {
            'Authorization': 'Bearer ' + getCookie('access_token')
        },
        data: {
            name: $("#nameNN").val(),
            email: $("#emailNN").val(),
            sdt: $("#sdtNN").val(),
            diachi: $("#diachiNN").text()
        },
        success: function (response) {
            order();
        },
        error: function (error) {
            console.error(error);
            swal({
                title: "Có lỗi xảy ra!",
                text: "Vui lòng thử lại sau.",
                icon: "error"
            });
        },
        complete: function () {
            swal.close();
        }
    });
}

function order() {
    swal({
        title: "Đang thực hiện đơn hàng...",
        text: "Vui lòng chờ...",
        icon: "info",
        buttons: false,
        closeOnClickOutside: false,
        closeOnEsc: false
    });

    $.ajax({
        url: '/MuaHang/Order',
        type: 'POST',
        headers: {
            'Authorization': 'Bearer ' + getCookie('access_token')
        },
        success: function (response) {
            $('#thongtindathang').modal('hide');
            $('#giohangCanvas').offcanvas('hide');
            swal({
                title: "Thành công!",
                text: "Đơn hàng đã được đặt.",
                icon: "success"
            });
        },
        error: function (error) {
            console.error(error);
            swal({
                title: "Có lỗi xảy ra!",
                text: "Vui lòng thử lại sau.",
                icon: "error"
            });
        },
        complete: function () {
            swal.close();
        }
    });
}



