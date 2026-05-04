import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

function Notifications() {
    const [notifications, setNotifications] = useState([]);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7105/notificationHub", {
                withCredentials: true
            })
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveNotification", (message) => {
            setNotifications(prev => [message, ...prev]);
        });

        connection.start()
            .then(() => console.log("Conectat la notificari"))
            .catch(err => console.error(err));

        return () => {
            connection.stop();
        };
    }, []);

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Notificari</h2>

            {notifications.length === 0 && (
                <p>Nu ai notificari.</p>
            )}

            {notifications.map((n, index) => (
                <p key={index}>{n}</p>
            ))}
        </div>
    );
}

export default Notifications;