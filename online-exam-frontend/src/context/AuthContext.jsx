// src/context/AuthContext.jsx
import { createContext, useContext, useState } from 'react';
import { jwtDecode } from 'jwt-decode';

const AuthContext = createContext(null);

function getRoleFromToken(token) {
    try {
        const decoded = jwtDecode(token);
        return (
            decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
            decoded.role ||
            null
        );
    } catch {
        return null;
    }
}

export function AuthProvider({ children }) {
    const [user, setUser] = useState(() => {
        const token = localStorage.getItem('token');
        if (!token) return null;
        return { token, role: getRoleFromToken(token) };
    });

    const login = (token, refreshToken) => {
        localStorage.setItem('token', token);
        localStorage.setItem('refreshToken', refreshToken);
        setUser({ token, role: getRoleFromToken(token) });
    };

    const logout = () => {
        localStorage.clear();
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
}

export const useAuth = () => useContext(AuthContext);