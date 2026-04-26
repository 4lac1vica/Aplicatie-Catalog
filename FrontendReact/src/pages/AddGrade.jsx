import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function AddGrade() {
    const [students, setStudents] = useState([]);
    const [materii, setMaterii] = useState([]);

    const [studentId, setStudentId] = useState("");
    const [materieId, setMaterieId] = useState("");
    const [valoare, setValoare] = useState("");

    const [message, setMessage] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        const loadData = async () => {
            try {
                const response = await fetch("https://localhost:7105/api/grades/add-data", {
                    method: "GET",
                    credentials: "include"
                });

                const raw = await response.text();

                if (!response.ok) {
                    setMessage(raw || "Nu se pot incarca datele.");
                    return;
                }

                const data = JSON.parse(raw);

                setStudents(data.students || []);
                setMaterii(data.materii || []);
            } catch (err) {
                console.error(err);
                setMessage("Eroare la server.");
            }
        };

        loadData();
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch("https://localhost:7105/api/grades/add", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: "include",
                body: JSON.stringify({
                    studentId: Number(studentId),
                    materieId: Number(materieId),
                    valoare: Number(valoare)
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
                setMessage(data.message || "Nu s-a putut adauga nota.");
                return;
            }

            setMessage(data.message || "Nota adaugata cu succes.");
            setStudentId("");
            setMaterieId("");
            setValoare("");
        } catch (err) {
            console.error(err);
            setMessage("Eroare la server.");
        }
    };

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Adauga Nota</h2>

            <form onSubmit={handleSubmit}>
                <select
                    value={studentId}
                    onChange={(e) => setStudentId(e.target.value)}
                    required
                >
                    <option value="">Alege student</option>
                    {students.map((student) => (
                        <option key={student.id} value={student.id}>
                            {student.fullName}
                        </option>
                    ))}
                </select>

                <br /><br />

                <select
                    value={materieId}
                    onChange={(e) => setMaterieId(e.target.value)}
                    required
                >
                    <option value="">Alege materie</option>
                    {materii.map((materie) => (
                        <option key={materie.id} value={materie.id}>
                            {materie.nume}
                        </option>
                    ))}
                </select>

                <br /><br />

                <input
                    type="number"
                    placeholder="Nota"
                    value={valoare}
                    min="1"
                    max="10"
                    onChange={(e) => setValoare(e.target.value)}
                    required
                />

                <br /><br />

                <button type="submit">
                    Adauga Nota
                </button>
            </form>

            <p>{message}</p>

            <button onClick={() => navigate("/teacher")}>
                Inapoi
            </button>
        </div>
    );
}

export default AddGrade;