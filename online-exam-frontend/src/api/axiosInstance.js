// src/api/axiosInstance.js
import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:9050',
});

// هر request اتوماتیک token رو میزنه
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');  
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// اگه 401 گرفت، refresh کن
api.interceptors.response.use(
  (res) => res,
  async (error) => {
    const original = error.config;
    if (error.response?.status === 401 && !original._retry) {
      original._retry = true;
      try {
        const refreshToken = localStorage.getItem('refreshToken');
        const { data } = await axios.post(
          'https://localhost:9050/Api/User/RefreshToken',
          { refreshToken }
        );
        localStorage.setItem('token', data.data.token);
localStorage.setItem('refreshToken', data.data.refreshToken);
original.headers.Authorization = `Bearer ${data.data.token}`;
        return api(original);
      } catch {
        localStorage.clear();
        window.location.href = '/login';
      }
    }
    return Promise.reject(error);
  }
);

export default api;