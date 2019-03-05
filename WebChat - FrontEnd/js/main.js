// All function and code javascript and jquery here
function loadRegister() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
      if (this.readyState == 4 && this.status == 200) {
        document.getElementById("login").innerHTML =
        this.responseText;
      }
    };
    
    xhttp.send();
  }