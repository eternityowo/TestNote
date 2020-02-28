"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/noteHub").build();

// Disable send button until connection is established
// document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message;//.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;

    $('.toast').toast('show');

    //var li = document.createElement("li");
    //li.textContent = encodedMsg;
    //document.getElementById("messagesList").appendChild(li);
});

//connection.start().then(function () {
//    document.getElementById("sendButton").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});

//document.getElementById("createNote").addEventListener("click", function (event) {
//    var user = 'userInput'// document.getElementById("userInput").value;
//    var message = 'messageInput'// document.getElementById("messageInput").value;
//    connection.invoke("SendNotification", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});