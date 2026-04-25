import { useState } from "react";

function Register() {
    const [form, setForm] = useState({
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        role: "Student",
        grupa: "",
        materie: "",
        telephone: ""
    });

    const [message, setMessage] = useState("");

    const handleChange = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value
        });
    };

    const handleRegister = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch("https://localhost:7105/api/account/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(form)
            });

            const data = await response.json();

            if (!response.ok) {
                setMessage("Eroare: " + JSON.stringify(data));
                return;
            }

            setMessage("Cont creat cu succes!");

        } catch (err) {
            console.error(err);
            setMessage("Eroare server");
        }
    };

    return (
        <div style={{ textAlign: "center" }}>
            <h2>Register</h2>

            <form onSubmit={handleRegister}>
                <input name="firstName" placeholder="First Name" onChange={handleChange} /><br /><br />
                <input name="lastName" placeholder="Last Name" onChange={handleChange} /><br /><br />
                <input name="email" placeholder="Email" onChange={handleChange} /><br /><br />
                <input name="password" type="password" placeholder="Password" onChange={handleChange} /><br /><br />
                <input name="telephone" placeholder="Telefon" onChange={handleChange} /><br /><br />

                <select name="role" onChange={handleChange}>
                    <option value="Student">Student</option>
                    <option value="Teacher">Teacher</option>
                </select><br /><br />

                <input name="grupa" placeholder="Grupa (student)" onChange={handleChange} /><br /><br />
                <input name="materie" placeholder="Materie (teacher)" onChange={handleChange} /><br /><br />

                <button type="submit">Register</button>
            </form>

            <p>{message}</p>
        </div>
    );
}

export default Register;