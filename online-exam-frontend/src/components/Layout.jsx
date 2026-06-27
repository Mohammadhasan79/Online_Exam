// src/components/Layout.jsx
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const navItems = [
  { path: '/dashboard', label: '🏠 داشبورد' },
  { path: '/exams', label: '📋 آزمون‌ها' },
];

export default function Layout({ children }) {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div style={styles.wrapper}>
      {/* Sidebar */}
      <aside style={styles.sidebar}>
        <div style={styles.logo}>🎓 آزمون آنلاین</div>
        <nav>
          {navItems.map((item) => (
            <Link
              key={item.path}
              to={item.path}
              style={{
                ...styles.navItem,
                ...(location.pathname === item.path ? styles.navItemActive : {}),
              }}
            >
              {item.label}
            </Link>
          ))}
        </nav>
        <button style={styles.logoutBtn} onClick={handleLogout}>
          🚪 خروج
        </button>
      </aside>

      {/* Main */}
      <main style={styles.main}>
        {children}
      </main>
    </div>
  );
}

const styles = {
  wrapper: {
    display: 'flex',
    minHeight: '100vh',
    fontFamily: 'Tahoma, sans-serif',
    direction: 'rtl',
    background: '#f0f2f5',
  },
  sidebar: {
    width: '220px',
    background: '#1e1b4b',
    display: 'flex',
    flexDirection: 'column',
    padding: '1.5rem 1rem',
    gap: '0.5rem',
    position: 'fixed',
    height: '100vh',
    right: 0,
  },
  logo: {
    color: '#fff',
    fontSize: '18px',
    fontWeight: 'bold',
    marginBottom: '2rem',
    textAlign: 'center',
    padding: '0.5rem',
    borderBottom: '1px solid #3730a3',
    paddingBottom: '1rem',
  },
  navItem: {
    display: 'block',
    color: '#a5b4fc',
    textDecoration: 'none',
    padding: '10px 14px',
    borderRadius: '8px',
    marginBottom: '4px',
    fontSize: '14px',
    transition: 'background 0.2s',
  },
  navItemActive: {
    background: '#4f46e5',
    color: '#fff',
  },
  logoutBtn: {
    marginTop: 'auto',
    background: 'transparent',
    border: '1px solid #4f46e5',
    color: '#a5b4fc',
    padding: '10px',
    borderRadius: '8px',
    cursor: 'pointer',
    fontSize: '14px',
  },
  main: {
    marginRight: '220px',
    padding: '2rem',
    flex: 1,
  },
};