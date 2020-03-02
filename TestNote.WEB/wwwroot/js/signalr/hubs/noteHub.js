"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/noteHub")
    .build();

connection.start();

connection.on("ReceiveMessage", function (user, message) {
    var msg = message;
    var encodedMsg = user + " says " + msg;

    Notify('fa fa-check-circle', "Create note", user + " created new note", "success");
    //$('.toast').toast('show');
});