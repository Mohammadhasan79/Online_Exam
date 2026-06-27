// src/pages/RegisterPage.jsx
import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import api from '../api/axiosInstance';

export default function RegisterPage() {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    universityCode: '',
    password: '',
    firstName: '',
    lastName: '',
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (e) =>
    setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      const { data } = await api.post('/Api/User/Register', form);
      if (data.success) {
        navigate('/login');
      } else {
        setError(data.message);
      }
    } catch (err) {
      setError(err.response?.data?.message || 'خطا در ثبت‌نام');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <h2 style={styles.title}>ثبت‌نام</h2>
        <form onSubmit={handleSubmit}>
          <input
            style={styles.input}
            name="firstName"
            placeholder="نام"
            value={form.firstName}
            onChange={handleChange}
            required
          />
          <input
            style={styles.input}
            name="lastName"
            placeholder="نام خانوادگی"
            value={form.lastName}
            onChange={handleChange}
            required
          />
          <input
            style={styles.input}
            name="universityCode"
            placeholder="کد دانشگاهی"
            value={form.universityCode}
            onChange={handleChange}
            required
          />
          <input
            style={styles.input}
            name="password"
            type="password"
            placeholder="رمز عبور"
            value={form.password}
            onChange={handleChange}
            required
          />
          {error && <p style={styles.error}>{error}</p>}
          <button style={styles.button} disabled={loading}>
            {loading ? 'در حال ثبت‌نام...' : 'ثبت‌نام'}
          </button>
        </form>
        <p style={styles.link}>
          حساب دارید؟ <Link to="/login">ورود</Link>
        </p>
      </div>
    </div>
  );
}

const styles = {
  container: {
    minHeight: '100vh',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    background: '#f0f2f5',
    fontFamily: 'Tahoma, sans-serif',
    direction: 'rtl',
  },
  card: {
    background: '#fff',
    padding: '2rem',
    borderRadius: '12px',
    boxShadow: '0 2px 16px rgba(0,0,0,0.1)',
    width: '340px',
  },
  title: { textAlign: 'center', marginBottom: '1.5rem', color: '#333' },
  input: {
    width: '100%',
    padding: '10px 12px',
    marginBottom: '1rem',
    borderRadius: '8px',
    border: '1px solid #ddd',
    fontSize: '14px',
    boxSizing: 'border-box',
  },
  button: {
    width: '100%',
    padding: '10px',
    background: '#4f46e5',
    color: '#fff',
    border: 'none',
    borderRadius: '8px',
    fontSize: '15px',
    cursor: 'pointer',
  },
  error: { color: 'red', fontSize: '13px', marginBottom: '0.5rem' },
  link: { textAlign: 'center', marginTop: '1rem', fontSize: '13px' },
};