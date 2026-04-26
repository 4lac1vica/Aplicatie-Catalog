import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function MyGrades() {
    const [grades, setGrades] = useState([]);
    const [message, setMessage] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        const loadGrades = async () => {
            try {
                const response = await fetch("https://localhost:7105/api/grades/my-grades", {
                    method: "GET",
                    credentials: "include"
                });

                const raw = await response.text();

                if (!response.ok) {
                    setMessage(raw || "Nu se pot incarca notele.");
                    return;
                }

                const data = raw ? JSON.parse(raw) : [];
                setGrades(data);
            } catch (err) {
                console.error(err);
                setMessage("Eroare la server.");
            }
        };

        loadGrades();
    }, []);

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Notele Mele</h2>

            {message && <p style={{ color: "red" }}>{message}</p>}

            {grades.length === 0 && !message && (
                <p>Nu ai note momentan.</p>
            )}

            {grades.length > 0 && (
                <table border="1" style={{ margin: "20px auto", padding: "10px" }}>
                    <thead>
                        <tr>
                            <th>Materie</th>
                            <th>Profesor</th>
                            <th>Nota</th>
                        </tr>
                    </thead>
                    <tbody>
                        {grades.map((grade, index) => (
                            <tr key={index}>
                                <td>{grade.materie || grade.materieNume}</td>
                                <td>{grade.profesor || grade.profesorNume}</td>
                                <td>{grade.valoare}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}

            <button onClick={() => navigate("/student")}>
                Inapoi
            </button>
        </div>
    );
}

export default MyGrades;