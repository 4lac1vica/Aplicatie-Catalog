import { useState } from "react";
import { useNavigate } from "react-router-dom";

function Search() {
    const [query, setQuery] = useState("");
    const [results, setResults] = useState([]);
    const [message, setMessage] = useState("");
    const navigate = useNavigate();

    const handleSearch = async (e) => {
        e.preventDefault();

        if (!query.trim()) {
            setResults([]);
            return;
        }

        try {
            const response = await fetch(
                `https://localhost:7105/api/search/users?query=${encodeURIComponent(query)}`,
                {
                    method: "GET",
                    credentials: "include"
                }
            );

            const raw = await response.text();

            if (!response.ok) {
                setMessage(raw || "Eroare la cautare.");
                return;
            }

            const data = raw ? JSON.parse(raw) : [];
            setResults(data);
            setMessage("");

        } catch (err) {
            console.error(err);
            setMessage("Eroare la server.");
        }
    };

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Cauta utilizatori</h2>

            <form onSubmit={handleSearch}>
                <input
                    type="text"
                    placeholder="Introdu nume sau email..."
                    value={query}
                    onChange={(e) => setQuery(e.target.value)}
                    style={{ padding: "8px", width: "250px" }}
                />

                <br /><br />

                <button type="submit">
                    Cauta
                </button>
            </form>

            {message && <p style={{ color: "red" }}>{message}</p>}

            <div style={{ marginTop: "30px" }}>
                {results.length === 0 && query && (
                    <p>Nu s-au gasit rezultate.</p>
                )}

                {results.length > 0 && (
                    <table border="1" style={{ margin: "0 auto", padding: "10px" }}>
                        <thead>
                            <tr>
                                <th>Nume</th>
                                <th>Email</th>
                                <th>Rol</th>
                            </tr>
                        </thead>
                        <tbody>
                            {results.map((user, index) => (
                                <tr key={index}>
                                    <td>
                                        {user.firstName} {user.lastName}
                                    </td>
                                    <td>{user.email}</td>
                                    <td>{user.role}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </div>

            <br />

            <button onClick={() => navigate(-1)}>
                Inapoi
            </button>
        </div>
    );
}

export default Search;