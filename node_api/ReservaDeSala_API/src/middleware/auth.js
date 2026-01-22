import jwt from 'jsonwebtoken';

export const auth = (req, res, next) => {
    if (process.env.AUTH_DISABLED === 'true') return next(); // bypass
    const header = req.headers.authorization || '';
    const [, token] = header.split(' ');
    if (!token) return res.status(401).json({ error: 'Token ausente' });
    try {
        const payload = jwt.verify(token, process.env.JWT_SECRET);
        req.user = payload; // { id, email, perfil }
        return next();
    } catch (err) {
        return res.status(401).json({ error: 'Token inválido ou expirado' });
    }
};

export const requireRole = (role) => (req, res, next) => {
    if (!req.user || req.user.perfil !== role) return res.status(403).json({ error: 'Acesso negado' });
    return next();
};