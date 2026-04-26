import { useNavigate } from "react-router-dom";

function TeacherDashboard() {
    const navigate = useNavigate();

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h1>Dashboard Teacher</h1>

            <button onClick={() => navigate("/add-grade")}>
                Adauga Nota
            </button>

            <br /><br />

            <button onClick={() => navigate("/profile")}>
                Profilul Meu
            </button>

            <br /><br />

            <button
                onClick={() => navigate("/delete-account")}
                style={{ backgroundColor: "red", color: "white" }}
            >
                Sterge Cont
            </button>

            <br /><br />

            <button
                onClick={() => navigate(-2)}
                style={{ backgroundColor: "green", color: "white" }}
            >
                Logout
            </button>

            <button onClick={() => navigate("/search")}>
                Cauta utilizatori
            </button>

        </div>
    );
}

export default TeacherDashboard;