"use strict";

function moveToSwagger() {
    window.location.href = "help";
}

function inputChanged(input: HTMLInputElement) {
    input.classList.remove("error");
    var myLabel = document.getElementById(input.dataset.label);
    myLabel.classList.remove("error");
}

function validateClick(button: HTMLButtonElement) : boolean {
    let requiredElements = document.getElementsByClassName('required');

    let message = "";

    for(let i = 0; i <requiredElements.length; i++) {
        let requiredElement = requiredElements[i] as HTMLInputElement;
        if (requiredElement.value === '') {
            requiredElement.classList.add("error");
            
            var myLabel = document.getElementById(requiredElement.dataset.label);
            myLabel.classList.add("error");

            message += requiredElement.dataset.errorMessage;
            message += "\n";
        }
    }

    if (message === "") {
            return true;
        } else {
            alert(message);
            return false;
        }
}

function getHost(): string {
    return location.protocol + '//' + location.host;
}

function loginClick(userBox: HTMLInputElement, passBox: HTMLInputElement) {
    let host = getHost() + '/api/Users/Login';
    let xhr = new XMLHttpRequest();
    xhr.open("POST", host, true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && (xhr.status >= 200 && xhr.status < 300))
    } 
}

function apiClick(button: HTMLButtonElement) {

}