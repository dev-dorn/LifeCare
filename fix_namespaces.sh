#!/bin/bash

# Fix Shared.Domain namespaces
find LifeCare.Modules/Shared/Domain -name "*.cs" -type f -exec sed -i 's/namespace LifeCare.Domain/namespace LifeCare.Modules.Shared.Domain/g' {} \;
find LifeCare.Modules/Shared/Domain -name "*.cs" -type f -exec sed -i 's/using LifeCare.Domain/using LifeCare.Modules.Shared.Domain/g' {} \;

# Fix Shared.Application namespaces
find LifeCare.Modules/Shared/Application -name "*.cs" -type f -exec sed -i 's/namespace LifeCare.Application/namespace LifeCare.Modules.Shared.Application/g' {} \;
find LifeCare.Modules/Shared/Application -name "*.cs" -type f -exec sed -i 's/using LifeCare.Application/using LifeCare.Modules.Shared.Application/g' {} \;

# Fix Shared.Infrastructure namespaces
find LifeCare.Modules/Shared/Infrastructure -name "*.cs" -type f -exec sed -i 's/namespace LifeCare.Infrastructure/namespace LifeCare.Modules.Shared.Infrastructure/g' {} \;
find LifeCare.Modules/Shared/Infrastructure -name "*.cs" -type f -exec sed -i 's/using LifeCare.Infrastructure/using LifeCare.Modules.Shared.Infrastructure/g' {} \;

# Fix Patients.Domain namespaces
find LifeCare.Modules/Patients/Domain -name "*.cs" -type f -exec sed -i 's/namespace LifeCare.Domain.Patients/namespace LifeCare.Modules.Patients.Domain/g' {} \;
find LifeCare.Modules/Patients/Domain -name "*.cs" -type f -exec sed -i 's/using LifeCare.Domain.Patients/using LifeCare.Modules.Patients.Domain/g' {} \;
find LifeCare.Modules/Patients/Domain -name "*.cs" -type f -exec sed -i 's/using LifeCare.Domain.Common/using LifeCare.Modules.Shared.Domain.Common/g' {} \;
find LifeCare.Modules/Patients/Domain -name "*.cs" -type f -exec sed -i 's/using LifeCare.Domain;/using LifeCare.Modules.Shared.Domain;/g' {} \;

# Fix Patients.Application namespaces
find LifeCare.Modules/Patients/Application -name "*.cs" -type f -exec sed -i 's/namespace LifeCare.Application.Patients/namespace LifeCare.Modules.Patients.Application/g' {} \;
find LifeCare.Modules/Patients/Application -name "*.cs" -type f -exec sed -i 's/using LifeCare.Application.Patients/using LifeCare.Modules.Patients.Application/g' {} \;
find LifeCare.Modules/Patients/Application -name "*.cs" -type f -exec sed -i 's/using LifeCare.Application.Interfaces/using LifeCare.Modules.Shared.Application.Interfaces/g' {} \;
find LifeCare.Modules/Patients/Application -name "*.cs" -type f -exec sed -i 's/using LifeCare.Application.common/using LifeCare.Modules.Shared.Application.common/g' {} \;
find LifeCare.Modules/Patients/Application -name "*.cs" -type f -exec sed -i 's/using LifeCare.Domain.Patients/using LifeCare.Modules.Patients.Domain/g' {} \;
find LifeCare.Modules/Patients/Application -name "*.cs" -type f -exec sed -i 's/using LifeCare.Domain.Common/using LifeCare.Modules.Shared.Domain.Common/g' {} \;

# Fix Patients.Infrastructure namespaces
find LifeCare.Modules/Patients/Infrastructure -name "*.cs" -type f -exec sed -i 's/namespace LifeCare.Infrastructure/namespace LifeCare.Modules.Patients.Infrastructure/g' {} \;
find LifeCare.Modules/Patients/Infrastructure -name "*.cs" -type f -exec sed -i 's/using LifeCare.Infrastructure/using LifeCare.Modules.Patients.Infrastructure/g' {} \;
find LifeCare.Modules/Patients/Infrastructure -name "*.cs" -type f -exec sed -i 's/using LifeCare.Domain.Patients/using LifeCare.Modules.Patients.Domain/g' {} \;
find LifeCare.Modules/Patients/Infrastructure -name "*.cs" -type f -exec sed -i 's/using LifeCare.Domain.Common/using LifeCare.Modules.Shared.Domain.Common/g' {} \;
find LifeCare.Modules/Patients/Infrastructure -name "*.cs" -type f -exec sed -i 's/using LifeCare.Application.Patients/using LifeCare.Modules.Patients.Application/g' {} \;
find LifeCare.Modules/Patients/Infrastructure -name "*.cs" -type f -exec sed -i 's/using LifeCare.Application.Interfaces/using LifeCare.Modules.Shared.Application.Interfaces/g' {} \;

echo "✅ Namespaces fixed!"
