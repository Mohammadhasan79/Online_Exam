// src/pages/AssignStudentsPage.jsx
import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Layout from '../components/Layout';
import examApi from '../api/examApi';

export default function AssignStudentsPage() {
    const { examId } = useParams();
    const navigate = useNavigate();

    const [students, setStudents] = useState([]);
    const [assigns, setAssigns] = useState([]);
    const [examName, setExamName] = useState('');
    const [selectedStudent, setSelectedStudent] = useState('');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [actionError, setActionError] = useState('');

    const fetchData = async () => {
        setLoading(true);
        try {
            const [examUserRes, assignRes] = await Promise.all([
                examApi.getUserAndExamList(),
                examApi.getStudentAssignList(),
            ]);

            const userList = examUserRes.data.data?.userList || [];
            const examsList = examUserRes.data.data?.examsList || [];
            const allAssigns = assignRes.data.data || [];

            setStudents(userList);
            setExamName(
                examsList.find((e) => e.examId === Number(examId))?.examName || ''
            );
            setAssigns(allAssigns.filter((a) => a.examId === Number(examId)));
        } catch (err) {
            setError(err.response?.data?.message || 'خطا در دریافت اطلاعات');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, [examId]);

    const handleAssign = async (e) => {
        e.preventDefault();
        if (!selectedStudent) return;
        setActionError('');
        try {
            await examApi.addStudentAssign(selectedStudent, Number(examId));
            setSelectedStudent('');
            fetchData();
        } catch (err) {
            setActionError(err.response?.data?.message || 'خطا در تخصیص دانشجو');
        }
    };

    const handleRemove = async (studentAssignId) => {
        if (!window.confirm('حذف این تخصیص؟')) return;
        try {
            await examApi.deleteStudentAssign(studentAssignId);
            setAssigns(assigns.filter((a) => a.studentAssignId !== studentAssignId));
        } catch (err) {
            alert(err.response?.data?.message || 'خطا در حذف');
        }
    };

    // دانشجوهایی که هنوز این آزمون به‌شون تخصیص داده نشده
    const assignedIds = new Set(
        assigns.map((a) => a.userName) // اگه userId نداریم، فعلاً با userName تشخیص می‌دیم
    );
    const availableStudents = students.filter(
        (s) => !assigns.some((a) => a.userName === s.userName)
    );

    return (
        <Layout>
            <button style={styles.backBtn} onClick={() => navigate('/exams')}>
                ← بازگشت به آزمون‌ها
            </button>
            <h1 style={styles.title}>تخصیص دانشجو — {examName}</h1>

            {loading && <p>در حال بارگذاری...</p>}
            {error && <p style={styles.error}>{error}</p>}

            {!loading && (
                <>
                    <form onSubmit={handleAssign} style={styles.assignForm}>
                        <select
                            style={styles.select}
                            value={selectedStudent}
                            onChange={(e) => setSelectedStudent(e.target.value)}
                        >
                            <option value="">انتخاب دانشجو...</option>
                            {availableStudents.map((s) => (
                                <option key={s.userId} value={s.userId}>
                                    {s.firstName} {s.lastName} ({s.userName})
                                </option>
                            ))}
                        </select>
                        <button type="submit" style={styles.assignBtn} disabled={!selectedStudent}>
                            ➕ تخصیص بده
                        </button>
                    </form>
                    {actionError && <p style={styles.error}>{actionError}</p>}

                    <h3 style={styles.subTitle}>دانشجویان دارای این آزمون</h3>
                    <div style={styles.list}>
                        {assigns.map((a) => (
                            <div key={a.studentAssignId} style={styles.listItem}>
                                <span>
                                    {a.firstName} ({a.userName})
                                </span>
                                <button
                                    style={styles.removeBtn}
                                    onClick={() => handleRemove(a.studentAssignId)}
                                >
                                    حذف
                                </button>
                            </div>
                        ))}
                        {assigns.length === 0 && (
                            <p style={styles.empty}>هنوز دانشجویی برای این آزمون انتخاب نشده.</p>
                        )}
                    </div>
                </>
            )}
        </Layout>
    );
}

const styles = {
    backBtn: {
        background: 'none',
        border: 'none',
        color: '#4f46e5',
        cursor: 'pointer',
        fontSize: '13px',
        marginBottom: '1rem',
        padding: 0,
    },
    title: { fontSize: '22px', color: '#1e1b4b', marginBottom: '1.5rem' },
    assignForm: {
        display: 'flex',
        gap: '0.75rem',
        marginBottom: '1.5rem',
    },
    select: {
        flex: 1,
        padding: '10px 12px',
        borderRadius: '8px',
        border: '1px solid #ddd',
        fontSize: '14px',
        fontFamily: 'Tahoma, sans-serif',
    },
    assignBtn: {
        background: '#4f46e5',
        color: '#fff',
        border: 'none',
        padding: '10px 18px',
        borderRadius: '8px',
        cursor: 'pointer',
        fontSize: '14px',
    },
    subTitle: { fontSize: '16px', color: '#1e1b4b', marginBottom: '0.75rem' },
    list: {
        background: '#fff',
        borderRadius: '12px',
        padding: '0.5rem',
        boxShadow: '0 2px 8px rgba(0,0,0,0.07)',
    },
    listItem: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: '10px 14px',
        borderBottom: '1px solid #f3f4f6',
        fontSize: '14px',
    },
    removeBtn: {
        background: '#fee2e2',
        color: '#dc2626',
        border: 'none',
        padding: '6px 12px',
        borderRadius: '6px',
        cursor: 'pointer',
        fontSize: '12px',
    },
    error: { color: 'red', fontSize: '13px', marginBottom: '1rem' },
    empty: { color: '#6b7280', fontSize: '13px', padding: '1rem' },
};