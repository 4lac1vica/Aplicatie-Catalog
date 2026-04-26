import { useEffect, useState } from "react";

function AdminDashboard() {
    const API = "https://localhost:7105/api/admin";

    const [term, setTerm] = useState("");
    const [users, setUsers] = useState([]);
    const [materii, setMaterii] = useState([]);
    const [numeMaterie, setNumeMaterie] = useState("");
    const [message, setMessage] = useState("");

    const readResponse = async (response) => {
        const raw = await response.text();

        console.log("STATUS:", response.status);
        console.log("RAW:", raw);

        try {
            return raw ? JSON.parse(raw) : {};
        } catch {
            return { message: raw };
        }
    };

    const loadMaterii = async () => {
        const response = await fetch(`${API}/materii`, {
            credentials: "include"
        });

        const data = await readResponse(response);

        if (!response.ok) {
            setMessage(data.message || `Eroare materii. Status: ${response.status}`);
            return;
        }

        setMaterii(Array.isArray(data) ? data : []);
    };

    const searchUsers = async () => {
        const response = await fetch(`${API}/users?term=${encodeURIComponent(term)}`, {
            credentials: "include"
        });

        const data = await readResponse(response);

        if (!response.ok) {
            setMessage(data.message || `Eroare cautare useri. Status: ${response.status}`);
            return;
        }

        setUsers(Array.isArray(data) ? data : []);
    };

    const addMaterie = async (e) => {
        e.preventDefault();

        const response = await fetch(`${API}/materii`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "include",
            body: JSON.stringify({
                nume: numeMaterie
            })
        });

        const data = await readResponse(response);

        if (!response.ok) {
            setMessage(data.message || `Eroare adaugare materie. Status: ${response.status}`);
            return;
        }

        setMessage(data.message || "Materia a fost adaugata.");
        setNumeMaterie("");
        loadMaterii();
    };

    const deleteMaterie = async (id) => {
        const response = await fetch(`${API}/materii/${id}`, {
            method: "DELETE",
            credentials: "include"
        });

        const data = await readResponse(response);

        if (!response.ok) {
            setMessage(data.message || `Eroare stergere materie. Status: ${response.status}`);
            return;
        }

        setMessage(data.message || "Materia a fost stearsa.");
        loadMaterii();
    };

    const deleteUser = async (id) => {
        const response = await fetch(`${API}/users/${id}`, {
            method: "DELETE",
            credentials: "include"
        });

        const data = await readResponse(response);

        if (!response.ok) {
            setMessage(data.message || `Eroare stergere user. Status: ${response.status}`);
            return;
        }

        setMessage(data.message || "User sters.");
        searchUsers();
    };

    useEffect(() => {
        const start = async () => {
            await loadMaterii();
        };

        start();
    }, []);

    return (
        <div style={{ textAlign: "center", marginTop: "40px" }}>
            <h1>Admin Dashboard</h1>

            {message && <p style={{ color: "red" }}>{message}</p>}

            <hr />

            <h2>Cauta utilizatori</h2>

            <input
                type="text"
                placeholder="Email, nume..."
                value={term}
                onChange={(e) => setTerm(e.target.value)}
            />

            <br /><br />

            <button onClick={searchUsers}>Cauta</button>

            <br /><br />

            {users.map((user) => (
                <div key={user.id} style={{ marginBottom: "10px" }}>
                    <span>{user.email}</span>{" "}
                    <button
                        onClick={() => deleteUser(user.id)}
                        style={{ backgroundColor: "red", color: "white" }}
                    >
                        Sterge
                    </button>
                </div>
            ))}

            <hr />

            <h2>Adauga materie</h2>

            <form onSubmit={addMaterie}>
                <input
                    type="text"
                    placeholder="Nume materie"
                    value={numeMaterie}
                    onChange={(e) => setNumeMaterie(e.target.value)}
                />

                <br /><br />

                <button type="submit">Adauga</button>
            </form>

            <hr />

            <h2>Materii existente</h2>

            {materii.map((materie) => (
                <div key={materie.id} style={{ marginBottom: "10px" }}>
                    <span>{materie.nume}</span>{" "}
                    <button
                        onClick={() => deleteMaterie(materie.id)}
                        style={{ backgroundColor: "red", color: "white" }}
                    >
                        Sterge
                    </button>
                </div>
            ))}
        </div>
    );
}

export default AdminDashboard;