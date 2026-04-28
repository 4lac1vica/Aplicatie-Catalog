import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function MyAbsente() {
    const API = "https://localhost:7105/api";

    const [absente, setAbsente] = useState([]);
    const [message, setMessage] = useState("");

    const navigate = useNavigate();

    const readResponse = async (response) => {
        const raw = await response.text();

        try {
            return raw ? JSON.parse(raw) : {};
        } catch {
            return { message: raw };
        }
    };

    useEffect(() => {
        const loadAbsente = async () => {
            try {
                const response = await fetch(`${API}/Absente/my-absences`, {
                    method: "GET",
                    credentials: "include"
                });

                const data = await readResponse(response);

                if (!response.ok) {
                    setMessage(data.message || `Eroare. Status: ${response.status}`);
                    return;
                }

                setAbsente(Array.isArray(data) ? data : []);
            } catch (err) {
                console.error(err);
                setMessage("Eroare la incarcarea absentelor.");
            }
        };

        loadAbsente();
    }, []);

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Absentele Mele</h2>

            {message && <p style={{ color: "red" }}>{message}</p>}

            {absente.length === 0 && !message && (
                <p>Nu ai absente.</p>
            )}

            {absente.map((absenta) => (
                <div
                    key={absenta.id}
                    style={{
                        border: "1px solid gray",
                        margin: "10px auto",
                        padding: "10px",
                        width: "350px"
                    }}
                >
                    <p><b>Materie:</b> {absenta.materie}</p>
                    <p><b>Profesor:</b> {absenta.profesor}</p>
                    <p>
                        <b>Data:</b>{" "}
                        {absenta.data
                            ? new Date(absenta.data).toLocaleDateString()
                            : "Nespecificata"}
                    </p>
                </div>
            ))}

            <button onClick={() => navigate(-1)}>Inapoi</button>
        </div>
    );
}

export default MyAbsente;