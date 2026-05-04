import { useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";

function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [message, setMessage] = useState("");

    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    const role = searchParams.get("role") || "";

    const handleLogin = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch("https://localhost:7105/api/account/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: "include",
                body: JSON.stringify({
                    email,
                    password,
                    role
                })
            });

            const raw = await response.text();
            console.log("STATUS:", response.status);
            console.log("RAW RESPONSE:", raw);

            let data = {};
            try {
                data = JSON.parse(raw);
            } catch {
                data = { message: raw };
            }

            if (!response.ok) {
                setMessage(data.message || "Login failed");
                return;
            }

            if (data.role === "Student") {
                navigate("/student");
            } else if (data.role === "Teacher") {
                navigate("/teacher");
            } else if (data.role === "Admin") {
                navigate("/admin");
            } else {
                navigate("/");
            }

        } catch (err) {
            console.error(err);
            setMessage("Eroare la server");
        }
    };

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Login {role && `- ${role}`}</h2>

            <form onSubmit={handleLogin}>
                <input
                    type="email"
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <br /><br />

                <input
                    type="password"
                    placeholder="Parola"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <br /><br />

                <button type="submit">Login</button>
            </form>

            <p style={{ color: "red" }}>{message}</p>

            <button onClick={() => navigate("/")}>
                Inapoi la Home
            </button>

        </div>
    );
}

export default Login;