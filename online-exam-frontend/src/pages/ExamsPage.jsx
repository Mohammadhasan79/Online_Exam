// src/pages/ExamsPage.jsx
import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import Layout from '../components/Layout';
import { useAuth } from '../context/AuthContext';
import examApi from '../api/examApi';

export default function ExamsPage() {
    const { user } = useAuth();
    const isProf = user?.role === 'Prof';

    return (
        <Layout>
            {isProf ? <ProfExamsView /> : <StudentExamsView />}
        </Layout>
    );
}

// ---------- نمای استاد ----------
function ProfExamsView() {
    const [exams, setExams] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const fetchExams = async () => {
        setLoading(true);
        try {
            const { data } = await examApi.getExamByUserId();
            setExams(data.data || []);
        } catch (err) {
            setError(err.response?.data?.message || 'خطا در دریافت آزمون‌ها');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchExams();
    }, []);

    const handleDelete = async (examId) => {
        if (!window.confirm('آیا از حذف این آزمون مطمئن هستید؟')) return;
        try {
            await examApi.deleteExam(examId);
            setExams(exams.filter((e) => e.examId !== examId));
        } catch (err) {
            alert(err.response?.data?.message || 'خطا در حذف آزمون');
        }
    };

    return (
        <div>
            <div style={styles.headerRow}>
                <h1 style={styles.title}>آزمون‌های من</h1>
                <Link to="/exams/new" style={styles.addBtn}>
                    ➕ ساخت آزمون جدید
                </Link>
            </div>

            {loading && <p>در حال بارگذاری...</p>}
            {error && <p style={styles.error}>{error}</p>}

            <div style={styles.grid}>
                {exams.map((exam) => (
                    <div key={exam.examId} style={styles.card}>
                        <h3 style={styles.cardTitle}>{exam.examName}</h3>
                        <p style={styles.cardDesc}>{exam.examDescription}</p>
                        <p style={styles.cardMeta}>
                            ⏱ {exam.examTime} دقیقه &nbsp;|&nbsp; 📅{' '}
                            {new Date(exam.startTime).toLocaleString('fa-IR')}
                        </p>
                        <p style={styles.cardMeta}>
                            ❓ {exam.question?.length || 0} سوال
                        </p>
                        <div style={styles.cardActions}>
                            <Link to={`/exams/${exam.examId}/questions`} style={styles.actionBtn}>
                                مدیریت سوالات
                            </Link>
                            <Link
                                className="btn btn-primary"
                                to={`/exams/${exam.examId}/students`}
                            >
                                پاسخ‌های دانشجویان
                            </Link>
                            <Link to={`/exams/${exam.examId}/edit`} style={styles.actionBtn}>
                                ویرایش
                            </Link>
                            <Link
                                to={`/exams/${exam.examId}/assign`}
                                style={styles.actionBtn}
                            >
                                تخصیص دانشجو
                            </Link>
                            <button
                                style={styles.deleteBtn}
                                onClick={() => handleDelete(exam.examId)}
                            >
                                حذف
                            </button>
                        </div>
                    </div>
                ))}
            </div>

            {!loading && exams.length === 0 && (
                <p style={styles.empty}>هنوز آزمونی نساخته‌اید.</p>
            )}
        </div>
    );
}

// ---------- نمای دانشجو ----------
function StudentExamsView() {
    const [assigns, setAssigns] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchAssigns = async () => {
            try {
                const { data } = await examApi.getStudentAssignList();
                setAssigns(data.data || []);
            } catch (err) {
                setError(err.response?.data?.message || 'خطا در دریافت آزمون‌ها');
            } finally {
                setLoading(false);
            }
        };
        fetchAssigns();
    }, []);

    return (
        <div>
            <h1 style={styles.title}>آزمون‌های من</h1>

            {loading && <p>در حال بارگذاری...</p>}
            {error && <p style={styles.error}>{error}</p>}

            <div style={styles.grid}>
                {assigns.map((a) => (
                    <div key={a.studentAssignId} style={styles.card}>
                        <h3 style={styles.cardTitle}>{a.examName}</h3>
                        <p style={styles.cardMeta}>
                            ⏱ {a.examTime} دقیقه &nbsp;|&nbsp; 📅{' '}
                            {new Date(a.startTime).toLocaleString('fa-IR')}
                        </p>
                        <div style={styles.cardActions}>
                            <Link to={`/student/exams/${a.examId}`} style={styles.actionBtn}>
                                شروع آزمون
                            </Link>
                        </div>
                    </div>
                ))}
            </div>

            {!loading && assigns.length === 0 && (
                <p style={styles.empty}>هیچ آزمونی به شما تخصیص داده نشده است.</p>
            )}
        </div>
    );
}

const styles = {
    headerRow: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        marginBottom: '1.5rem',
    },
    title: { fontSize: '24px', color: '#1e1b4b', margin: 0 },
    addBtn: {
        background: '#4f46e5',
        color: '#fff',
        padding: '10px 16px',
        borderRadius: '8px',
        textDecoration: 'none',
        fontSize: '14px',
    },
    grid: {
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fill, minmax(260px, 1fr))',
        gap: '1.25rem',
    },
    card: {
        background: '#fff',
        borderRadius: '12px',
        padding: '1.25rem',
        boxShadow: '0 2px 8px rgba(0,0,0,0.07)',
    },
    cardTitle: { margin: '0 0 0.5rem', color: '#1e1b4b' },
    cardDesc: { fontSize: '13px', color: '#6b7280', margin: '0 0 0.5rem' },
    cardMeta: { fontSize: '12px', color: '#6b7280', margin: '0 0 0.3rem' },
    cardActions: {
        display: 'flex',
        gap: '0.5rem',
        flexWrap: 'wrap',
        marginTop: '1rem',
    },
    actionBtn: {
        background: '#eef2ff',
        color: '#4f46e5',
        padding: '6px 10px',
        borderRadius: '6px',
        fontSize: '12px',
        textDecoration: 'none',
    },
    deleteBtn: {
        background: '#fee2e2',
        color: '#dc2626',
        padding: '6px 10px',
        borderRadius: '6px',
        fontSize: '12px',
        border: 'none',
        cursor: 'pointer',
    },
    error: { color: 'red', marginBottom: '1rem' },
    empty: { color: '#6b7280', marginTop: '2rem' },
};