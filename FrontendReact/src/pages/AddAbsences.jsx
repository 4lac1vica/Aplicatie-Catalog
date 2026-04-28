import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function AddAbsenta() {
    const API = "https://localhost:7105/api";

    const [students, setStudents] = useState([]);
    const [materii, setMaterii] = useState([]);
    const [studentId, setStudentId] = useState("");
    const [materieId, setMaterieId] = useState("");
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
        const loadData = async () => {
            try {
                const response = await fetch(`${API}/Absente/add-absenta-data`, {
                    method: "GET",
                    credentials: "include"
                });

                const data = await readResponse(response);

                if (!response.ok) {
                    setMessage(data.message || `Eroare. Status: ${response.status}`);
                    return;
                }

                setStudents(Array.isArray(data.students) ? data.students : []);
                setMaterii(Array.isArray(data.materii) ? data.materii : []);
            } catch (err) {
                console.error(err);
                setMessage("Eroare la incarcarea datelor.");
            }
        };

        loadData();
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage("");

        if (!studentId || !materieId) {
            setMessage("Selecteaza studentul si materia.");
            return;
        }

        try {
            const response = await fetch(`${API}/Absente/add-absenta`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: "include",
                body: JSON.stringify({
                    studentId: Number(studentId),
                    materieId: Number(materieId)
                })
            });

            const data = await readResponse(response);

            if (!response.ok) {
                setMessage(data.message || `Eroare. Status: ${response.status}`);
                return;
            }

            setMessage(data.message || "Absenta a fost adaugata.");
            setStudentId("");
            setMaterieId("");
        } catch (err) {
            console.error(err);
            setMessage("Eroare server.");
        }
    };

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Adauga Absenta</h2>

            {message && <p style={{ color: "red" }}>{message}</p>}

            <form onSubmit={handleSubmit}>
                <select value={studentId} onChange={(e) => setStudentId(e.target.value)}>
                    <option value="">Selecteaza student</option>
                    {students.map((student) => (
                        <option key={student.id} value={student.id}>
                            {student.fullname}
                        </option>
                    ))}
                </select>

                <br /><br />

                <select value={materieId} onChange={(e) => setMaterieId(e.target.value)}>
                    <option value="">Selecteaza materia</option>
                    {materii.map((materie) => (
                        <option key={materie.id} value={materie.id}>
                            {materie.name}
                        </option>
                    ))}
                </select>

                <br /><br />

                <button type="submit">Adauga Absenta</button>
            </form>

            <br />

            <button onClick={() => navigate(-1)}>Inapoi</button>
        </div>
    );
}

export default AddAbsenta;