// src/pages/ExamFormPage.jsx
import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Layout from '../components/Layout';
import examApi from '../api/examApi';

export default function ExamFormPage() {
    const { examId } = useParams(); // اگه باشه یعنی حالت ویرایش
    const isEdit = !!examId;
    const navigate = useNavigate();

    const [form, setForm] = useState({
        examName: '',
        examDescription: '',
        startTime: '',
        examTime: 60,
    });
    const [loading, setLoading] = useState(false);
    const [fetching, setFetching] = useState(isEdit);
    const [error, setError] = useState('');

    useEffect(() => {
        if (!isEdit) return;
        const fetchExam = async () => {
            try {
                const { data } = await examApi.getExamById(examId);
                const exam = data.data;
                setForm({
                    examName: exam.examName,
                    examDescription: exam.examDescription,
                    // برای input type=datetime-local باید فرمت YYYY-MM-DDTHH:mm باشه
                    startTime: exam.startTime?.slice(0, 16) || '',
                    examTime: exam.examTime,
                });
            } catch (err) {
                setError(err.response?.data?.message || 'خطا در دریافت آزمون');
            } finally {
                setFetching(false);
            }
        };
        fetchExam();
    }, [examId, isEdit]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm({ ...form, [name]: name === 'examTime' ? Number(value) : value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);
        try {
            const payload = {
                ...form,
                startTime: new Date(form.startTime).toISOString(),
            };
            if (isEdit) {
                await examApi.updateExam(examId, payload);
            } else {
                await examApi.createExam(payload);
            }
            navigate('/exams');
        } catch (err) {
            setError(err.response?.data?.message || 'خطا در ذخیره آزمون');
        } finally {
            setLoading(false);
        }
    };

    return (
        <Layout>
            <h1 style={styles.title}>
                {isEdit ? 'ویرایش آزمون' : 'ساخت آزمون جدید'}
            </h1>

            {fetching ? (
                <p>در حال بارگذاری...</p>
            ) : (
                <form onSubmit={handleSubmit} style={styles.form}>
                    <label style={styles.label}>نام آزمون</label>
                    <input
                        style={styles.input}
                        name="examName"
                        value={form.examName}
                        onChange={handleChange}
                        required
                    />

                    <label style={styles.label}>توضیحات</label>
                    <textarea
                        style={{ ...styles.input, minHeight: '90px' }}
                        name="examDescription"
                        value={form.examDescription}
                        onChange={handleChange}
                    />

                    <label style={styles.label}>زمان شروع</label>
                    <input
                        style={styles.input}
                        type="datetime-local"
                        name="startTime"
                        value={form.startTime}
                        onChange={handleChange}
                        required
                    />

                    <label style={styles.label}>مدت آزمون (دقیقه)</label>
                    <input
                        style={styles.input}
                        type="number"
                        name="examTime"
                        value={form.examTime}
                        onChange={handleChange}
                        required
                        min={1}
                    />

                    {error && <p style={styles.error}>{error}</p>}

                    <div style={styles.actions}>
                        <button type="submit" style={styles.submitBtn} disabled={loading}>
                            {loading ? 'در حال ذخیره...' : isEdit ? 'ذخیره تغییرات' : 'ساخت آزمون'}
                        </button>
                        <button
                            type="button"
                            style={styles.cancelBtn}
                            onClick={() => navigate('/exams')}
                        >
                            انصراف
                        </button>
                    </div>
                </form>
            )}
        </Layout>
    );
}

const styles = {
    title: { fontSize: '24px', color: '#1e1b4b', marginBottom: '1.5rem' },
    form: {
        background: '#fff',
        borderRadius: '12px',
        padding: '2rem',
        maxWidth: '480px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.07)',
        display: 'flex',
        flexDirection: 'column',
    },
    label: {
        fontSize: '13px',
        color: '#374151',
        marginBottom: '0.4rem',
        fontWeight: 'bold',
    },
    input: {
        padding: '10px 12px',
        marginBottom: '1.2rem',
        borderRadius: '8px',
        border: '1px solid #ddd',
        fontSize: '14px',
        fontFamily: 'Tahoma, sans-serif',
        boxSizing: 'border-box',
    },
    actions: { display: 'flex', gap: '0.75rem' },
    submitBtn: {
        background: '#4f46e5',
        color: '#fff',
        border: 'none',
        padding: '10px 20px',
        borderRadius: '8px',
        cursor: 'pointer',
        fontSize: '14px',
    },
    cancelBtn: {
        background: '#f3f4f6',
        color: '#374151',
        border: 'none',
        padding: '10px 20px',
        borderRadius: '8px',
        cursor: 'pointer',
        fontSize: '14px',
    },
    error: { color: 'red', fontSize: '13px', marginBottom: '1rem' },
};