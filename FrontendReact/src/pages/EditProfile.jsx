import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function EditProfile() {
    const [telephone, setTelephone] = useState("");
    const [grupa, setGrupa] = useState("");
    const [materie, setMaterie] = useState("");
    const [password, setPassword] = useState("");
    const [role, setRole] = useState("");
    const [message, setMessage] = useState("");

    const navigate = useNavigate();

    useEffect(() => {
        const loadProfile = async () => {
            const response = await fetch("https://localhost:7105/api/account/profile", {
                method: "GET",
                credentials: "include"
            });

            const data = await response.json();

            setTelephone(data.telephone || "");
            setGrupa(data.grupa || "");
            setMaterie(data.materie || "");
            setRole(data.role || "");
        };

        loadProfile();
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const response = await fetch("https://localhost:7105/api/account/profile", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "include",
            body: JSON.stringify({
                telephone,
                grupa,
                materie,
                password
            })
        });

        const raw = await response.text();

        let data = {};
        try {
            data = JSON.parse(raw);
        } catch {
            data = { message: raw };
        }

        if (!response.ok) {
            setMessage(data.message || "Eroare la actualizare.");
            return;
        }

        setMessage("Profil actualizat cu succes!");

        setTimeout(() => {
            navigate("/profile");
        }, 1000);
    };

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Editeaza Profil</h2>

            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    placeholder="Telefon"
                    value={telephone}
                    onChange={(e) => setTelephone(e.target.value)}
                />

                <br /><br />

                {role === "Student" && (
                    <>
                        <input
                            type="text"
                            placeholder="Grupa"
                            value={grupa}
                            onChange={(e) => setGrupa(e.target.value)}
                        />
                        <br /><br />
                    </>
                )}

                {role === "Teacher" && (
                    <>
                        <input
                            type="text"
                            placeholder="Materie"
                            value={materie}
                            onChange={(e) => setMaterie(e.target.value)}
                        />
                        <br /><br />
                    </>
                )}

                <input
                    type="password"
                    placeholder="Parola pentru confirmare"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />

                <br /><br />

                <button type="submit">
                    Salveaza
                </button>
            </form>

            <br />

            <button onClick={() => navigate("/profile")}>
                Inapoi
            </button>

            <p>{message}</p>
        </div>
    );
}

export default EditProfile;