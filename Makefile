# ============================================================================
# LifeCare Hospital Management System - Makefile
# ============================================================================

# Variables
PROJECT_NAME := life-care
API_PROJECT := LifeCare.API
TEST_PROJECT := LifeCare.TestBench
SOLUTION := LifeCare.sln

# Docker
DOCKER_COMPOSE := docker-compose
DOCKER := docker

# Colors
RED := \033[0;31m
GREEN := \033[0;32m
YELLOW := \033[1;33m
BLUE := \033[0;34m
NC := \033[0m # No Color

# ============================================================================
# Development Commands
# ============================================================================

.PHONY: help
help: ## Show this help message
	@echo "$(BLUE)🏥 LifeCare Hospital Management System$(NC)"
	@echo "$(BLUE)=====================================$(NC)"
	@echo ""
	@echo "$(YELLOW)Development Commands (without Docker):$(NC)"
	@echo "  make build           Build the solution"
	@echo "  make run             Run the API locally"
	@echo "  make test            Run tests locally"
	@echo "  make clean           Clean build artifacts"
	@echo ""
	@echo "$(YELLOW)Docker Commands:$(NC)"
	@echo "  make docker-build    Build Docker images"
	@echo "  make docker-up       Start services with Docker"
	@echo "  make docker-dev      Start development environment"
	@echo "  make docker-test     Run tests in Docker"
	@echo "  make docker-logs     View Docker logs"
	@echo "  make docker-down     Stop Docker services"
	@echo ""
	@echo "$(YELLOW)Utility Commands:$(NC)"
	@echo "  make setup           Setup development environment"
	@echo "  make status          Show system status"
	@echo "  make prune           Clean up Docker resources"
	@echo "  make check-naming    Check naming conventions"
	@echo ""
	@echo "$(YELLOW)Shortcuts:$(NC)"
	@echo "  make dev             Build and run locally"
	@echo "  make ci              Build and test (CI/CD)"
	@echo "  make all             Build and run with Docker"
	@echo ""

.PHONY: build
build: ## Build the solution
	@echo "$(BLUE)🏗️  Building solution...$(NC)"
	dotnet build $(SOLUTION)
	@echo "$(GREEN)✓ Build completed$(NC)"

.PHONY: run
run: ## Run the API locally
	@echo "$(BLUE)🚀 Starting API...$(NC)"
	@echo "$(YELLOW)API will be available at:$(NC)"
	@echo "  • http://localhost:8080"
	@echo "  • https://localhost:8081"
	@echo "  • Swagger UI: http://localhost:8080/swagger"
	@echo ""
	dotnet run --project $(API_PROJECT)

.PHONY: test
test: ## Run tests locally
	@echo "$(BLUE)🧪 Running tests...$(NC)"
	dotnet run --project $(TEST_PROJECT)

.PHONY: clean
clean: ## Clean build artifacts
	@echo "$(BLUE)🧹 Cleaning build artifacts...$(NC)"
	dotnet clean
	rm -rf */bin */obj
	@echo "$(GREEN)✓ Clean completed$(NC)"

# ============================================================================
# Docker Commands
# ============================================================================

.PHONY: docker-build
docker-build: ## Build Docker images
	@echo "$(BLUE)🐳 Building Docker images...$(NC)"
	$(DOCKER_COMPOSE) build --no-cache
	@echo "$(GREEN)✓ Docker images built$(NC)"

.PHONY: docker-up
docker-up: ## Start services with Docker
	@echo "$(BLUE)🐳 Starting services...$(NC)"
	$(DOCKER_COMPOSE) up -d
	@echo "$(GREEN)✓ Services started$(NC)"
	@echo "$(YELLOW)API: http://localhost:8080$(NC)"
	@echo "$(YELLOW)Health: http://localhost:8080/health$(NC)"
	@echo ""
	@echo "$(BLUE)View logs with: make docker-logs$(NC)"

.PHONY: docker-dev
docker-dev: ## Start development environment with hot reload
	@echo "$(BLUE)🐳 Starting development environment...$(NC)"
	$(DOCKER_COMPOSE) -f docker-compose.yml -f docker-compose.override.yml up --build

.PHONY: docker-test
docker-test: ## Run tests in Docker
	@echo "$(BLUE)🐳 Running tests in Docker...$(NC)"
	$(DOCKER_COMPOSE) --profile test up test-bench --build --abort-on-container-exit
	@echo ""
	@echo "$(GREEN)✓ Tests completed$(NC)"

.PHONY: docker-logs
docker-logs: ## View Docker logs
	@echo "$(BLUE)📋 Showing API logs (Ctrl+C to exit)...$(NC)"
	@$(DOCKER_COMPOSE) logs -f api

.PHONY: docker-logs-all
docker-logs-all: ## View all Docker logs
	@echo "$(BLUE)📋 Showing all logs (Ctrl+C to exit)...$(NC)"
	@$(DOCKER_COMPOSE) logs -f

.PHONY: docker-down
docker-down: ## Stop Docker services
	@echo "$(BLUE)🐳 Stopping services...$(NC)"
	$(DOCKER_COMPOSE) down
	@echo "$(GREEN)✓ Services stopped$(NC)"

.PHONY: docker-restart
docker-restart: docker-down docker-up ## Restart Docker services

# ============================================================================
# Utility Commands
# ============================================================================

.PHONY: setup
setup: ## Setup development environment
	@echo "$(BLUE)🔧 Setting up development environment...$(NC)"
	@echo ""
	
	@# Check .NET SDK
	@echo "$(BLUE)Checking .NET SDK...$(NC)"
	@if ! command -v dotnet >/dev/null 2>&1; then \
		echo "$(RED)❌ .NET SDK not found. Please install .NET 9.0$(NC)"; \
		echo "  Visit: https://dotnet.microsoft.com/download/dotnet/9.0"; \
		exit 1; \
	else \
		DOTNET_VERSION=$$(dotnet --version); \
		echo "$(GREEN)✓ .NET SDK found: $$DOTNET_VERSION$(NC)"; \
	fi
	@echo ""
	
	@# Check Docker (optional)
	@echo "$(BLUE)Checking Docker...$(NC)"
	@if ! command -v docker >/dev/null 2>&1; then \
		echo "$(YELLOW)⚠️  Docker not found (optional for local development)$(NC)"; \
	else \
		DOCKER_VERSION=$$(docker --version); \
		echo "$(GREEN)✓ Docker found: $$DOCKER_VERSION$(NC)"; \
	fi
	@echo ""
	
	@# Generate SSL certificate
	@echo "$(BLUE)🔐 Setting up SSL certificate...$(NC)"
	@if [ ! -f "$$HOME/.aspnet/https/aspnetapp.pfx" ]; then \
		mkdir -p "$$HOME/.aspnet/https"; \
		dotnet dev-certs https -ep "$$HOME/.aspnet/https/aspnetapp.pfx" -p password; \
		dotnet dev-certs https --trust; \
		echo "$(GREEN)✓ SSL certificate generated$(NC)"; \
	else \
		echo "$(GREEN)✓ SSL certificate already exists$(NC)"; \
	fi
	@echo ""
	
	@# Create directories
	@echo "$(BLUE)📁 Creating data directories...$(NC)"
	@mkdir -p data logs
	@echo "$(GREEN)✓ Directories created: data/, logs/$(NC)"
	@echo ""
	
	@# Restore packages
	@echo "$(BLUE)📦 Restoring NuGet packages...$(NC)"
	@dotnet restore $(SOLUTION)
	@echo "$(GREEN)✓ Packages restored$(NC)"
	@echo ""
	
	@echo "$(GREEN)🎉 Setup completed successfully!$(NC)"
	@echo ""
	@echo "$(YELLOW)Next steps:$(NC)"
	@echo "  Run locally:       $(BLUE)make run$(NC)"
	@echo "  Run with Docker:   $(BLUE)make docker-up$(NC)"
	@echo "  Run tests:         $(BLUE)make test$(NC)"
	@echo "  View all commands: $(BLUE)make help$(NC)"

.PHONY: status
status: ## Show system status
	@echo "$(BLUE)📊 LifeCare System Status$(NC)"
	@echo "$(BLUE)========================$(NC)"
	@echo ""
	@echo "$(YELLOW)Docker Containers:$(NC)"
	@$(DOCKER_COMPOSE) ps 2>/dev/null || echo "  $(RED)Docker Compose not available$(NC)"
	@echo ""
	@echo "$(YELLOW)Docker Images:$(NC)"
	@$(DOCKER) images life-care-* 2>/dev/null || echo "  $(RED)No LifeCare images found$(NC)"
	@echo ""
	@echo "$(YELLOW)Endpoints:$(NC)"
	@echo "  • API:           http://localhost:8080"
	@echo "  • Swagger UI:    http://localhost:8080/swagger"
	@echo "  • Health Check:  http://localhost:8080/health"
	@echo ""
	@echo "$(YELLOW)Local Files:$(NC)"
	@echo "  • Database:      ./data/LifeCare.db"
	@echo "  • Logs:          ./logs/"

.PHONY: prune
prune: ## Clean up Docker resources
	@echo "$(BLUE)🧹 Pruning Docker resources...$(NC)"
	@echo "$(YELLOW)This will remove:$(NC)"
	@echo "  • Stopped containers"
	@echo "  • Unused networks"
	@echo "  • Dangling images"
	@echo "  • Build cache"
	@echo ""
	@read -p "Continue? [y/N] " -n 1 -r; \
	echo; \
	if [[ $$REPLY =~ ^[Yy]$$ ]]; then \
		$(DOCKER_COMPOSE) down -v --remove-orphans; \
		$(DOCKER) system prune -f; \
		echo "$(GREEN)✓ Docker resources pruned$(NC)"; \
	else \
		echo "$(YELLOW)Cancelled$(NC)"; \
	fi

# ============================================================================
# Shortcuts
# ============================================================================

.PHONY: dev
dev: build run ## Build and run locally (development workflow)

.PHONY: ci
ci: build test ## Build and test (CI/CD workflow)

.PHONY: all
all: docker-build docker-up ## Build and run with Docker

# ============================================================================
# Quality Checks
# ============================================================================

.PHONY: check-naming
check-naming: ## Check naming conventions
	@echo "$(BLUE)🔍 Checking naming conventions...$(NC)"
	@echo ""
	
	@# Check project naming
	@echo "$(YELLOW)Checking project files...$(NC)"
	@if [ ! -f "LifeCare.API/LifeCare.API.csproj" ]; then \
		echo "$(RED)❌ LifeCare.API/LifeCare.API.csproj not found$(NC)"; \
	else \
		echo "$(GREEN)✓ LifeCare.API/LifeCare.API.csproj$(NC)"; \
	fi
	
	@if [ ! -f "LifeCare.TestBench/LifeCare.TestBench.csproj" ]; then \
		echo "$(RED)❌ LifeCare.TestBench/LifeCare.TestBench.csproj not found$(NC)"; \
	else \
		echo "$(GREEN)✓ LifeCare.TestBench/LifeCare.TestBench.csproj$(NC)"; \
	fi
	
	@if [ ! -f "LifeCare.Application/LifeCare.Application.csproj" ]; then \
		echo "$(RED)❌ LifeCare.Application/LifeCare.Application.csproj not found$(NC)"; \
	else \
		echo "$(GREEN)✓ LifeCare.Application/LifeCare.Application.csproj$(NC)"; \
	fi
	
	@if [ ! -f "LifeCare.Domain/LifeCare.Domain.csproj" ]; then \
		echo "$(RED)❌ LifeCare.Domain/LifeCare.Domain.csproj not found$(NC)"; \
	else \
		echo "$(GREEN)✓ LifeCare.Domain/LifeCare.Domain.csproj$(NC)"; \
	fi
	
	@if [ ! -f "LifeCare.Infrastructure/LifeCare.Infrastructure.csproj" ]; then \
		echo "$(RED)❌ LifeCare.Infrastructure/LifeCare.Infrastructure.csproj not found$(NC)"; \
	else \
		echo "$(GREEN)✓ LifeCare.Infrastructure/LifeCare.Infrastructure.csproj$(NC)"; \
	fi
	@echo ""
	
	@# Check for inconsistent naming
	@echo "$(YELLOW)Checking for naming inconsistencies...$(NC)"
	@if [ -d "Lifecare.TestBench" ]; then \
		echo "$(RED)❌ Found: Lifecare.TestBench (should be LifeCare.TestBench)$(NC)"; \
		echo "$(YELLOW)  Fix: mv Lifecare.TestBench LifeCare.TestBench$(NC)"; \
	else \
		echo "$(GREEN)✓ No lowercase 'Lifecare' directories found$(NC)"; \
	fi
	@echo ""
	
	@# Check target framework
	@echo "$(YELLOW)Checking .NET target framework...$(NC)"
	@if grep -q "net10.0" LifeCare.API/LifeCare.API.csproj 2>/dev/null; then \
		echo "$(RED)❌ Found net10.0 (should be net9.0)$(NC)"; \
		echo "$(YELLOW)  Run: find . -name '*.csproj' -exec sed -i 's/net10.0/net9.0/g' {} +$(NC)"; \
	elif grep -q "net9.0" LifeCare.API/LifeCare.API.csproj 2>/dev/null; then \
		echo "$(GREEN)✓ Using net9.0$(NC)"; \
	else \
		echo "$(YELLOW)⚠️  Could not determine target framework$(NC)"; \
	fi
	@echo ""
	
	@echo "$(GREEN)✓ Naming convention check completed$(NC)"

.PHONY: check-health
check-health: ## Check if API is healthy
	@echo "$(BLUE)🏥 Checking API health...$(NC)"
	@if curl -sf http://localhost:8080/health > /dev/null 2>&1; then \
		echo "$(GREEN)✓ API is healthy$(NC)"; \
		curl -s http://localhost:8080/health | jq . 2>/dev/null || curl -s http://localhost:8080/health; \
	else \
		echo "$(RED)❌ API is not responding$(NC)"; \
		echo "$(YELLOW)  Try: make docker-logs$(NC)"; \
	fi

# ============================================================================
# Development Helpers
# ============================================================================

.PHONY: watch
watch: ## Run API with hot reload
	@echo "$(BLUE)🔥 Starting API with hot reload...$(NC)"
	dotnet watch --project $(API_PROJECT) run

.PHONY: migrate
migrate: ## Run database migrations (if applicable)
	@echo "$(BLUE)🗃️  Running database migrations...$(NC)"
	dotnet ef database update --project $(API_PROJECT)

.PHONY: format
format: ## Format code
	@echo "$(BLUE)✨ Formatting code...$(NC)"
	dotnet format $(SOLUTION)
	@echo "$(GREEN)✓ Code formatted$(NC)"

# Default target
.DEFAULT_GOAL := help

.PHONY: ef-migration
ef-migration: ## Create EF migration
	@echo "$(BLUE)📦 Creating migration...$(NC)"
	@read -p "Migration name: " name; \
	docker exec -it life-care-api dotnet ef migrations add &&name --project /src/LifeCare.Infrastructure --startup-project /src/LifeCare.API
	