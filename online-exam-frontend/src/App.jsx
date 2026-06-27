// src/App.jsx
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import DashboardPage from './pages/DashboardPage';
import ExamsPage from './pages/ExamsPage';
import ExamFormPage from './pages/ExamFormPage';
import AssignStudentsPage from './pages/AssignStudentsPage';
import QuestionsPage from "./pages/QuestionsPage";
import QuestionFormPage from "./pages/QuestionFormPage";
import StudentExamPage from "./pages/StudentExamPage";
import StudentAnswerListPage from "./pages/StudentAnswerListPage";
import StudentAnswerPage from "./pages/StudentAnswerPage";
function PrivateRoute({ children }) {
    const { user } = useAuth();
    return user ? children : <Navigate to="/login" />;
}

export default function App() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/register" element={<RegisterPage />} />
                    <Route path="/dashboard" element={
                        <PrivateRoute><DashboardPage /></PrivateRoute>
                    } />
                    <Route path="/exams" element={
                        <PrivateRoute><ExamsPage /></PrivateRoute>
                    } />
                    <Route path="/exams/new" element={
                        <PrivateRoute><ExamFormPage /></PrivateRoute>
                    } />
                    <Route path="/exams/:examId/edit" element={
                        <PrivateRoute><ExamFormPage /></PrivateRoute>
                    } />
                    <Route path="/exams/:examId/assign" element={
                        <PrivateRoute><AssignStudentsPage /></PrivateRoute>
                    } />
                    <Route
                        path="/exams/:examId/questions" element={
                            <PrivateRoute><QuestionsPage /></PrivateRoute>
                        }
                    />
                    <Route
                        path="/exams/:examId/students"
                        element={
                            <PrivateRoute>
                                <StudentAnswerListPage />
                            </PrivateRoute>
                        }
                    />
                    <Route
                        path="/exams/:examId/questions/new"
                        element={
                            <PrivateRoute>
                                <QuestionFormPage />
                            </PrivateRoute>
                        }
                    />

                    <Route
                        path="/exams/:examId/questions/:questionId/edit"
                        element={
                            <PrivateRoute>
                                <QuestionFormPage />
                            </PrivateRoute>
                        }
                    />
                    <Route
                        path="/student/exams/:examId"
                        element={<StudentExamPage />}
                    />
                    <Route
                        path="/exams/:examId/students/:studId"
                        element={
                            <PrivateRoute>
                                <StudentAnswerPage />
                            </PrivateRoute>
                        }
                    />
                    <Route path="*" element={<Navigate to="/dashboard" />} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    );
}