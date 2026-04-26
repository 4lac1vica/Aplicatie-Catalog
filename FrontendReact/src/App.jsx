import { BrowserRouter, Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import StudentDashboard from "./pages/StudentDashboard";
import TeacherDashboard from "./pages/TeacherDashboard";
import DeleteAccount from "./pages/DeleteAccount";
import Profile from "./pages/Profile";
import EditProfile from "./pages/EditProfile";
import About from "./pages/AboutUs";
import Privacy from "./pages/Privacy";
import MyGrades from "./pages/MyGrades";
import AddGrade from "./pages/AddGrade";
import Search from "./pages/Search";
import AdminDashboard from "./pages/AdminDashboard";

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/student" element={<StudentDashboard />} />
                <Route path="/teacher" element={<TeacherDashboard />} />
                <Route path="/delete-account" element={<DeleteAccount />} />
                <Route path="/profile" element={<Profile />} />
                <Route path="/edit-profile" element={<EditProfile />} />
                <Route path="/about" element={<About />} />
                <Route path="/privacy" element={<Privacy />} />
                <Route path="/my-grades" element={<MyGrades />} />
                <Route path="/add-grade" element={<AddGrade />} />
                <Route path="/search" element={<Search />} />
                <Route path="/admin" element={<AdminDashboard />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;