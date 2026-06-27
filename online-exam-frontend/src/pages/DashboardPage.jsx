// src/pages/DashboardPage.jsx
import { useAuth } from '../context/AuthContext';
import Layout from '../components/Layout';

export default function DashboardPage() {
  const { user } = useAuth();

  return (
    <Layout>
      <div style={styles.header}>
        <h1 style={styles.title}>خوش آمدید 👋</h1>
        <p style={styles.subtitle}>از منوی سمت راست بخش مورد نظر را انتخاب کنید</p>
      </div>

      <div style={styles.cards}>
        <div style={styles.card}>
          <div style={styles.cardIcon}>📋</div>
          <div style={styles.cardTitle}>آزمون‌ها</div>
          <div style={styles.cardDesc}>مدیریت و مشاهده آزمون‌ها</div>
        </div>
        <div style={styles.card}>
          <div style={styles.cardIcon}>❓</div>
          <div style={styles.cardTitle}>سوالات</div>
          <div style={styles.cardDesc}>افزودن و ویرایش سوالات</div>
        </div>
        <div style={styles.card}>
          <div style={styles.cardIcon}>✅</div>
          <div style={styles.cardTitle}>پاسخ‌ها</div>
          <div style={styles.cardDesc}>مشاهده نتایج و پاسخ‌ها</div>
        </div>
      </div>
    </Layout>
  );
}

const styles = {
  header: {
    marginBottom: '2rem',
  },
  title: {
    fontSize: '26px',
    color: '#1e1b4b',
    margin: 0,
  },
  subtitle: {
    color: '#6b7280',
    marginTop: '0.5rem',
  },
  cards: {
    display: 'flex',
    gap: '1.5rem',
    flexWrap: 'wrap',
  },
  card: {
    background: '#fff',
    borderRadius: '12px',
    padding: '1.5rem',
    width: '180px',
    boxShadow: '0 2px 8px rgba(0,0,0,0.07)',
    textAlign: 'center',
    cursor: 'pointer',
    transition: 'transform 0.2s',
  },
  cardIcon: { fontSize: '32px', marginBottom: '0.75rem' },
  cardTitle: { fontWeight: 'bold', color: '#1e1b4b', marginBottom: '0.4rem' },
  cardDesc: { fontSize: '12px', color: '#6b7280' },
};