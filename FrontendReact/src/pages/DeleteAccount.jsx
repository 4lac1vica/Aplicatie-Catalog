import { useState } from "react";
import { useNavigate } from "react-router-dom";

function DeleteAccount() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [message, setMessage] = useState("");

    const navigate = useNavigate();

    const handleDelete = async (e) => {
        e.preventDefault();

        if (!email.trim() || !password.trim()) {
            setMessage("Introdu emailul si parola.");
            return;
        }

        const confirmed = window.confirm("Esti sigur ca vrei sa stergi contul?");
        if (!confirmed) return;

        try {
            const response = await fetch("https://localhost:7105/api/account/delete", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: "include",
                body: JSON.stringify({
                    email: email,
                    password: password
                })
            });

            const raw = await response.text();

            console.log("DELETE STATUS:", response.status);
            console.log("DELETE RAW:", raw);

            let data = {};
            if (raw) {
                try {
                    data = JSON.parse(raw);
                } catch {
                    data = { message: raw };
                }
            }

            if (!response.ok) {
                setMessage(data.message || "Eroare la stergere.");
                return;
            }

            setMessage(data.message || "Cont sters cu succes.");

            localStorage.clear();
            sessionStorage.clear();

            setTimeout(() => {
                navigate("/");
            }, 1500);

        } catch (err) {
            console.error("DELETE ERROR:", err);
            setMessage("Eroare server.");
        }
    };

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Sterge Cont</h2>

            <p style={{ color: "red" }}>
                Aceasta actiune este permanenta!
            </p>

            <form onSubmit={handleDelete}>
                <input
                    type="email"
                    placeholder="Introdu emailul"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />

                <br /><br />

                <input
                    type="password"
                    placeholder="Introdu parola"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />

                <br /><br />

                <button
                    type="submit"
                    style={{ backgroundColor: "red", color: "white" }}
                >
                    Sterge Cont
                </button>
            </form>

            <br />

            <button onClick={() => navigate(-1)}>
                Inapoi
            </button>

            <p>{message}</p>
        </div>
    );
}

export default DeleteAccount;