import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

function StudentDashboard() {
    const navigate = useNavigate();
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

    const handleLogout = async () => {
        await fetch("https://localhost:7105/api/account/logout", {
            method: "POST",
            credentials: "include"
        });

        navigate("/login?role=Student");
    };

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h1>Dashboard Student</h1>

            <div style={{ marginBottom: "20px" }}>
                <h3>Notificari</h3>

                {notifications.length === 0 && <p>Nu ai notificari.</p>}

                {notifications.map((n, index) => (
                    <div
                        key={index}
                        style={{
                            border: "1px solid gray",
                            margin: "5px auto",
                            padding: "5px",
                            width: "300px"
                        }}
                    >
                        {n}
                    </div>
                ))}
            </div>

            <button onClick={() => navigate("/my-grades")}>Vezi Note</button>
            <button onClick={() => navigate("/profile")}>Profilul Meu</button>

            <button
                onClick={() => navigate("/delete-account")}
                style={{ backgroundColor: "red", color: "white" }}
            >
                Sterge Cont
            </button>

            <button
                onClick={handleLogout}
                style={{ backgroundColor: "green", color: "white" }}
            >
                Logout
            </button>

            <button onClick={() => navigate("/search")}>Cauta utilizatori</button>
            <button onClick={() => navigate("/my-absente")}>Vezi Absente</button>
        </div>
    );
}

export default StudentDashboard;