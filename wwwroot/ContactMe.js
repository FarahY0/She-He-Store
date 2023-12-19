
//document.addEventListener('DOMContentLoaded', function () {
//    const form = document.getElementById('my-form');
//    const successMessage = document.getElementById('success-message');

//    form.addEventListener('submit', function (event) {
//        event.preventDefault(); // Prevent the form from actually submitting

//        // Add your form submission logic here (e.g., sending data to a server)

//        // Once the form is successfully submitted, show the success message
//        successMessage.style.display = 'block';
//    });
//});

function changeColor() {
    var gfg = document.getElementById("name");
    gfg.style.color = "red";
};

function changeColore() {
    var ema = document.getElementById("email");
    ema.style.color = "red";
}

function changeColorem() {
    var mes = document.getElementById("message");
    mes.style.color = "red";
}

function suc(){

    {
        document.getElementById("success").innerHTML = "the form has been submitted successfully.";
    }


}

