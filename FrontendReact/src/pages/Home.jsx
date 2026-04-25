import { useNavigate } from "react-router-dom";

function Home() {
    const navigate = useNavigate();

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h1>Bine ai venit in cataLOG</h1>
            <p>Aplicatia ta pentru gestionarea catalogului.</p>

            <h2>Select the role you want to log in as:</h2>

            <button onClick={() => navigate("/login?role=Student")}>
                Student
            </button>

            <button onClick={() => navigate("/login?role=Teacher")}>
                Teacher
            </button>

            <h3>Don't have an account? Sign Up!</h3>

            <button onClick={() => navigate("/register")}>
                Sign Up
            </button>

            <h4>Continue anonymously</h4>

            <button onClick={() => navigate("/guest")}>
                Continue Unregistered
            </button>
        </div>
    );
}

export default Home;