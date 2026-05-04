import { useNavigate } from "react-router-dom";
import NotificationsList from "./NotificationsList";

function StudentDashboard() {
    const navigate = useNavigate();

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

            
            <NotificationsList />

            <br />

            <button onClick={() => navigate("/my-grades")}>
                Vezi Note
            </button>

            <button onClick={() => navigate("/profile")}>
                Profilul Meu
            </button>

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

            <button onClick={() => navigate("/search")}>
                Cauta utilizatori
            </button>

            <button onClick={() => navigate("/my-absente")}>
                Vezi Absente
            </button>
        </div>
    );
}

export default StudentDashboard;