import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

function NotificationsList() {
    const API = "https://localhost:7105/api";

    const [notifications, setNotifications] = useState([]);

    const readResponse = async (response) => {
        const raw = await response.text();
        try {
            return raw ? JSON.parse(raw) : [];
        } catch {
            return [];
        }
    };

    
    useEffect(() => {
        const loadNotifications = async () => {
            try {
                const response = await fetch(`${API}/Absente/notifications`, {
                    method: "GET",
                    credentials: "include"
                });

                const data = await readResponse(response);

                if (response.ok) {
                    setNotifications(Array.isArray(data) ? data : []);
                }
            } catch (err) {
                console.error(err);
            }
        };

        loadNotifications();
    }, []);

    
    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7105/notificationHub", {
                withCredentials: true
            })
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveNotification", (message) => {
            setNotifications(prev => [
                {
                    id: Date.now(),
                    message: message,
                    createdAt: new Date()
                },
                ...prev
            ]);
        });

        connection.start()
            .then(() => console.log("SignalR conectat"))
            .catch(err => console.error(err));

        return () => connection.stop();
    }, []);

    return (
        <div style={{ marginTop: "20px" }}>
            <h3>?? Notificari</h3>

            {notifications.length === 0 && (
                <p>Nu ai notificari.</p>
            )}

            {notifications.map((n) => (
                <div
                    key={n.id}
                    style={{
                        border: "1px solid gray",
                        padding: "10px",
                        margin: "10px auto",
                        width: "350px",
                        textAlign: "left",
                        borderRadius: "5px"
                    }}
                >
                    <p>{n.message}</p>

                    <small>
                        {n.createdAt
                            ? new Date(n.createdAt).toLocaleString()
                            : ""}
                    </small>
                </div>
            ))}
        </div>
    );
}

export default NotificationsList;