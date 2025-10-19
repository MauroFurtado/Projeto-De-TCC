const express = require('express');
const bodyParser = require('body-parser');
const routes = require('./routes/UsuarioRoutes');
const middleware = require('./middleware/index');

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(bodyParser.json());
app.use(middleware);

// Routes
app.use('/api', routes);

// Start the server
app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});