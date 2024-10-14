document.addEventListener("DOMContentLoaded", function () {


  

});

function dangnhap()
{
    $.ajax({
        url: '/Auth/LoginAdmin',
        type: 'POST',
        dataType: 'json',
        data: {
            name: $("#name").val(),
            password: $("#password").val()
        },
        success: function (response) {
            window.location.href = '/Admin/';
        },
        error: function (xhr, status, error) {
            alert("Sai mật khẩu")
        }
    });
}
