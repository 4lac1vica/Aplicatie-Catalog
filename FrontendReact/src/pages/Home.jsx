import { Link, useNavigate } from "react-router-dom";

function Home() {
    const navigate = useNavigate();

    const linkStyle = {
        color: "white",
        textDecoration: "none",
        marginRight: "15px",
        fontWeight: "bold"
    };

    return (
        <>

            <div
                style={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    padding: "15px 30px",
                    backgroundColor: "#111",
                    color: "white"
                }}
            >
                <h3 style={{ margin: 0 }}>Catalog App</h3>

                <div>
                    <Link to="/about" style={linkStyle}>
                        About Us
                    </Link>
                    <Link to="/privacy" style={linkStyle}>
                        Privacy
                    </Link>
                </div>
            </div>

            
            <div style={{ textAlign: "center", marginTop: "80px" }}>
                <h1>Bine ai venit!</h1>

                <p>Aplicatie pentru gestionarea catalogului scolar.</p>

                <div style={{ marginTop: "30px" }}>
                    <h2>Selecteaza rolul:</h2>

                    <button
                        onClick={() => navigate("/login?role=Student")}
                        style={{
                            padding: "10px 20px",
                            marginRight: "10px",
                            cursor: "pointer"
                        }}
                    >
                        Login as Student
                    </button>

                    <button
                        onClick={() => navigate("/login?role=Teacher")}
                        style={{
                            padding: "10px 20px",
                            cursor: "pointer"
                        }}
                    >
                        Login as Teacher
                    </button>

                    <h3 style={{ marginTop: "40px" }}>
                        Nu ai cont?
                    </h3>

                    <button
                        onClick={() => navigate("/register")}
                        style={{
                            padding: "10px 20px",
                            cursor: "pointer"
                        }}
                    >
                        Register
                    </button>
                </div>
            </div>
        </>
    );
}

export default Home;