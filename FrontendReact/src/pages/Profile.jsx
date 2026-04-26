import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function Profile() {
    const [profile, setProfile] = useState(null);
    const [message, setMessage] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        const loadProfile = async () => {
            try {
                const response = await fetch("https://localhost:7105/api/account/profile", {
                    method: "GET",
                    credentials: "include"
                });

                const raw = await response.text();

                console.log("PROFILE STATUS:", response.status);
                console.log("PROFILE RAW:", raw);

                if (!response.ok) {
                    setMessage(`Eroare profil. Status: ${response.status}`);
                    return;
                }

                if (!raw) {
                    setMessage("Backendul a returnat raspuns gol.");
                    return;
                }

                const data = JSON.parse(raw);
                setProfile(data);

            } catch (err) {
                console.error(err);
                setMessage("Eroare la incarcarea profilului");
            }
        };

        loadProfile();
    }, []);

    if (message) return <p style={{ color: "red" }}>{message}</p>;
    if (!profile) return <p>Se incarca...</p>;

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Profilul Meu</h2>

            <p><b>Nume:</b> {profile.firstName} {profile.lastName}</p>
            <p><b>Email:</b> {profile.email}</p>
            <p><b>Telefon:</b> {profile.telephone}</p>
            <p><b>Rol:</b> {profile.role}</p>

            {profile.role === "Student" && (
                <p><b>Grupa:</b> {profile.grupa}</p>
            )}

            {profile.role === "Teacher" && (
                <p><b>Materie:</b> {profile.materie}</p>
            )}

            <button onClick={() => navigate("/edit-profile")}>
                Editeaza Profil
            </button>

            <button onClick={() => navigate(-1)}>
                Inapoi
            </button>



        </div>
    );
}

export default Profile;